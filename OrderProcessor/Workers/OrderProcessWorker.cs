using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model;
using OrderProcessor.Configuration;

namespace OrderProcessor.Workers
{
	public class OrderProcessWorker : IHostedService
	{
		private readonly IDbContextFactory<OrdersProcessingDbContext> _dbContextFactory;
		private readonly OrderProcessConfiguration _processConfiguration;


		public OrderProcessWorker(
			IDbContextFactory<OrdersProcessingDbContext> dbContextFactory,
			IOptions<OrderProcessConfiguration> options)
		{
			_dbContextFactory = dbContextFactory ??
				throw new ArgumentNullException(nameof(dbContextFactory));

			_processConfiguration = options?.Value ??
				throw new ArgumentNullException(nameof(options));
		}
	
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				using (var context = await _dbContextFactory.CreateDbContextAsync())
				{
					var inWaitingOrder = await context.Orders
						.FirstOrDefaultAsync(x => x.Status == OrderStatuses.Waiting);

					var inWorkOrder = await context.Orders
						.FirstOrDefaultAsync(x => x.Status == OrderStatuses.InWork);

					if (inWaitingOrder != null)
					{
						inWaitingOrder.Status = OrderStatuses.InWork;
					}

					if(inWorkOrder != null)
					{
						var rnd = new Random().Next(1, 10);

						if(rnd >= 9)
						{
							inWorkOrder.Status = OrderStatuses.Cancelled;
						}
						else
						{
							inWorkOrder.Status = OrderStatuses.Completed;
						}
					}

					await context.SaveChangesAsync();

					await Task.Delay(_processConfiguration.Delay);
					
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
