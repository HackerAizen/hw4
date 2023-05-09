using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using OrderMicroservice.DTO;
using OrderMicroservice.Models.Requests;
using OrderMicroservice.Services;
using System.Diagnostics.Contracts;

namespace OrderMicroservice.Controllers
{
	/// <summary>
	/// Управление заказами
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class OrderController: Controller
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
		{
			_orderService = orderService ??
				throw new ArgumentNullException(nameof(orderService));
		}

		/// <summary>
		/// Создание заказа
		/// </summary>
		/// <param name="createOrderRequest"></param>
		/// <returns></returns>
		[HttpPost("Create")]
		[Authorize]
		public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest createOrderRequest)
		{
			createOrderRequest.UserId = int.Parse(User.FindFirst("UserId")?.Value);

			var response = await _orderService.CreateOrder(createOrderRequest);

			if (response.ReponseStatus == ServiceResponseStatuses.Sussess)
			{
				return Ok("Заказ успешно создан");
			}

			if (response.ReponseStatus == ServiceResponseStatuses.ValidationError)
			{
				return UnprocessableEntity(response.Message);
			}

			return BadRequest(response.Message);
		}

		/// <summary>
		/// Информация по заказу
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrderInfo(int id)
		{
			var response = await _orderService.GetOrderInfo(id);

			if (response.ReponseStatus == ServiceResponseStatuses.Sussess)
			{
				return Ok(response);
			}

			if (response.ReponseStatus == ServiceResponseStatuses.ValidationError)
			{
				return UnprocessableEntity(response.Message);
			}

			return BadRequest(response.Message);
		}
	}
}
