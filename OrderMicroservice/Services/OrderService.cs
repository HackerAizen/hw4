using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Entities;
using OrderMicroservice.DTO;
using OrderMicroservice.Models.Requests;
using OrderMicroservice.Models.Responses;

namespace OrderMicroservice.Services
{
	public class OrderService : IOrderService
	{
		private readonly IDbContextFactory<OrdersProcessingDbContext> _dbContextFactory;
		private readonly string[] _availableOrderStatuses =
			new string[] { OrderStatuses.Waiting, OrderStatuses.InWork, OrderStatuses.Completed, OrderStatuses.Cancelled };

		public OrderService(IDbContextFactory<OrdersProcessingDbContext> dbContextFactory)
		{
			_dbContextFactory = dbContextFactory ??
				throw new ArgumentNullException(nameof(dbContextFactory));
		}

		public async Task<ServiceResponse> CreateOrder(CreateOrderRequest createOrderRequest)
		{
			var response = await ValidateOrder(createOrderRequest);

			if (response.ReponseStatus != ServiceResponseStatuses.Sussess)
			{
				return response;
			}

			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				Order order = new Order()
				{
					UserId = createOrderRequest.UserId,
					SpecialRequests = createOrderRequest.SpecialRequests,
					Status = createOrderRequest.Status,
				};

				foreach (var dish in createOrderRequest.Dishes)
				{
					var dishEntity = await context.Dishes.FindAsync(dish.DishId);

					dishEntity.Quantity -= dish.Quantity;

					var orderDish = new OrderDish
					{
						DishId = dish.DishId,
						Quantity = dish.Quantity,
						Price = dish.Quantity * dishEntity.Price
					};

					order.OrderDishes.Add(orderDish);


				}

				await context.Orders.AddAsync(order);

				await context.SaveChangesAsync();

				return response;
			}
		}

		public async Task<OrderInfoResponse> GetOrderInfo(int id)
		{
			var response = new OrderInfoResponse()
			{
				ReponseStatus = ServiceResponseStatuses.Sussess
			};

			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				var order = await context.Orders.FindAsync(id);

				if (order == null)
				{
					response = new OrderInfoResponse()
					{
						ReponseStatus = ServiceResponseStatuses.ValidationError,
						Message = $"Заказ с Id: {id} не найден"
					};

					return response;
				}

				response.Id = order.Id;
				response.UserId = order.UserId;
				response.Status = order.Status;
				response.SpecialRequests = order.SpecialRequests;
				response.Created = order.Created;
				response.Updated = order.Updated;
				response.Dishes = order.OrderDishes.Select(x =>
					new OrderDishResponse
					{
						DishId = x.DishId,
						Price = x.Price,
						Quantity = x.Quantity
					})
					.ToArray();

				return response;
			}
		}

		private async Task<ServiceResponse> ValidateOrder(CreateOrderRequest createOrderRequest)
		{
			var response = new ServiceResponse
			{
				ReponseStatus = ServiceResponseStatuses.Sussess
			};

			if (!_availableOrderStatuses.Contains(createOrderRequest.Status))
			{
				response = new ServiceResponse
				{
					ReponseStatus = ServiceResponseStatuses.ValidationError,
					Message = $"Передан несуществующий статус заказа. Существующие: {string.Join(',', _availableOrderStatuses)}"
				};

				return response;
			}

			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				var user = await context.Users.FindAsync(createOrderRequest.UserId);

				if(user == null)
				{
					response = new ServiceResponse
					{
						ReponseStatus = ServiceResponseStatuses.ValidationError,
						Message = $"Пользователь с Id: {createOrderRequest.UserId} не найден"
					};

					return response;
				}

				if(!createOrderRequest.Dishes.Any())
				{
					response = new ServiceResponse
					{
						ReponseStatus = ServiceResponseStatuses.ValidationError,
						Message = $"Должно быть передано хотя бы одно блюдо"
					};

					return response;
				}

				if(createOrderRequest.Dishes.Count() != 
					createOrderRequest.Dishes.Select(x => x.DishId).Distinct().Count())
				{
					response = new ServiceResponse
					{
						ReponseStatus = ServiceResponseStatuses.ValidationError,
						Message = $"Нельзя передать одно блюдо больше одного раза"
					};

					return response;
				}

				foreach(var dish in createOrderRequest.Dishes)
				{
					if(dish.Quantity <= 0)
					{
						response = new ServiceResponse
						{
							ReponseStatus = ServiceResponseStatuses.ValidationError,
							Message = $"Количество блюда должно быть больше 0"
						};

						return response;
					}

					Dish? dishEntity = await context.Dishes.FindAsync(dish.DishId);

					if(dishEntity == null)
					{
						response = new ServiceResponse
						{
							ReponseStatus = ServiceResponseStatuses.ValidationError,
							Message = $"Блюда с Id: {dish.DishId} не существует"
						};

						return response;
					}

					if(dish.Quantity > dishEntity.Quantity)
					{
						response = new ServiceResponse
						{
							ReponseStatus = ServiceResponseStatuses.ValidationError,
							Message = $"В налиии имеется только: {dishEntity.Quantity} единиц блюда: {dish.DishId}"
						};

						return response;
					}
				}

				return response;

			}
		}
	}
}
