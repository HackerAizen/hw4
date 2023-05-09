using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Model;
using OrderProcessor.Configuration;
using OrderProcessor.Workers;
using System.Text.Json.Serialization;

namespace OrderProcessor
{
	public class Startup
	{
		private readonly IConfiguration _configuration;
		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers().AddJsonOptions(x =>
				x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


			services.AddDbContextFactory<OrdersProcessingDbContext>(options => options
						.UseLazyLoadingProxies()
						.UseNpgsql(_configuration.GetConnectionString("PostgreSql")));

			services.AddControllers();

			services.Configure<OrderProcessConfiguration>(_configuration.GetSection("OrderProcessConfiguration"));

			services.AddHostedService<OrderProcessWorker>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

		}
	}
}
