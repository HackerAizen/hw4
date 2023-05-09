using Microsoft.EntityFrameworkCore;
using Model;
using Model.Entities;
using OrderMicroservice.DTO;
using OrderMicroservice.Models.Requests;
using OrderMicroservice.Models.Responses;

namespace OrderMicroservice.Services
{
	public class DishService : IDishService
	{
		private readonly IDbContextFactory<OrdersProcessingDbContext> _dbContextFactory;

		public DishService(IDbContextFactory<OrdersProcessingDbContext> dbContextFactory)
		{
			_dbContextFactory = dbContextFactory ??
				throw new ArgumentNullException(nameof(dbContextFactory));
		}
		public async Task<ServiceResponse> CreateDishAsync(CreateDishRequest createDish)
		{
			ServiceResponse response = new ServiceResponse();

			if (!(createDish.Price > 0))
			{
				response = new ServiceResponse
				{
					ReponseStatus = ServiceResponseStatuses.ValidationError,
					Message = "Цена блюда должна быть больше 0"
				};

				return response;
			}

			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				Dish dish = new Dish()
				{
					Name = createDish.Name,
					Description = createDish.Description,
					IsAvailable = createDish.Quantity > 0,
					Quantity = createDish.Quantity,
					Price = createDish.Price,
				};

				await context.Dishes.AddAsync(dish);
				await context.SaveChangesAsync();

				return response;
			}

		}

		public async Task<ServiceResponse> DeleteDishAsync(int id)
		{
			var response = new ServiceResponse
			{
				ReponseStatus = ServiceResponseStatuses.Sussess
			};

			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				Dish? dish = await context.Dishes.FindAsync(id);

				if (dish == null)
				{
					response = new ServiceResponse
					{
						ReponseStatus = ServiceResponseStatuses.ValidationError,
						Message = $"Блюдо с Id: {id} не найдено"
					};

					return response;
				}

				if (dish.OrderDishes.Any())
				{
					response = new ServiceResponse
					{
						ReponseStatus = ServiceResponseStatuses.ValidationError,
						Message = $"Нельзя удалить блюдо, так как есть заказы, связанные" +
						$" с этим блюдом"
					};

					return response;
				}

				context.Dishes.Remove(dish);
				await context.SaveChangesAsync();

				return response;
			}
		}

		public async Task<DishResponse> GetDishInfoAsync(int id)
		{
			var response = new DishResponse
			{
				ReponseStatus = ServiceResponseStatuses.Sussess
			};

			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				Dish? dish = await context.Dishes.FindAsync(id);

				if (dish == null)
				{
					response = new DishResponse
					{
						ReponseStatus = ServiceResponseStatuses.ValidationError,
						Message = $"Блюдо с Id: {id} не найдено"
					};

					return response;
				}

				response.Quantity = dish.Quantity;
				response.Price = dish.Price;
				response.Description = dish.Description;
				response.Name = dish.Name;
				response.Id = id;
				response.IsAvailable = dish.IsAvailable;

				return response;
			}
		}

		public async Task<MenuResponse> GetMenuAsync()
		{
			var response = new MenuResponse
			{
				ReponseStatus = ServiceResponseStatuses.Sussess
			};

			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				response.Dishes = await context.Dishes.Where(x => x.IsAvailable)
					.Select(x =>
						 new DishResponse
						{
							Id = x.Id,
							Description = x.Description,
							IsAvailable = x.IsAvailable,
							Name = x.Name,
							Price = x.Price,
							Quantity = x.Quantity
						}
					).ToArrayAsync();

				return response;
			}
		}

		public async Task<ServiceResponse> UpdateDishAsync(UpdateDishRequest updateDishRequest)
		{
			var response = new DishResponse
			{
				ReponseStatus = ServiceResponseStatuses.Sussess
			};

			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				Dish? dish = await context.Dishes.FindAsync(updateDishRequest.Id);



				if (dish == null)
				{
					response = new DishResponse
					{
						ReponseStatus = ServiceResponseStatuses.ValidationError,
						Message = $"Блюдо с Id: {updateDishRequest.Id} не найдено"
					};

					return response;
				}

				dish.Name = updateDishRequest.Name;
				dish.Description = updateDishRequest.Description;
				dish.IsAvailable = updateDishRequest.Quantity > 0;
				dish.Quantity = updateDishRequest.Quantity;
				dish.Price = updateDishRequest.Price;

				await context.SaveChangesAsync();

				return response;

			}
		}

	}
}
