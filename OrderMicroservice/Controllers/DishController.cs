using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using OrderMicroservice.DTO;
using OrderMicroservice.Models.Requests;
using OrderMicroservice.Services;

namespace OrderMicroservice.Controllers
{
	/// <summary>
	/// Управление блюдами
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class DishController: Controller
	{
		private readonly IDishService _dishService;

		public DishController(IDishService dishService)
		{
			_dishService = dishService ??
				throw new ArgumentNullException(nameof(dishService));
		}

		/// <summary>
		/// Получение блюда по Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<ActionResult> GetById(int id)
		{
			var response = await _dishService.GetDishInfoAsync(id);

			if (response.ReponseStatus == ServiceResponseStatuses.Sussess)
			{
				return Ok(response);
			}

			if (response.ReponseStatus == ServiceResponseStatuses.ValidationError)
			{
				UnprocessableEntity(response.Message);
			}

			return BadRequest(response.Message);
		}

		/// <summary>
		/// Создание блюда
		/// </summary>
		/// <returns></returns>
		[HttpPost("Create")]
		[Authorize(Roles = ApplicationRoles.Manager)]
		public async Task<IActionResult> CreateDish([FromBody] CreateDishRequest createDishRequest)
		{
			if (!ModelState.IsValid)
			{
				return UnprocessableEntity(ModelState);
			}

			var response = await _dishService.CreateDishAsync(createDishRequest);

			if (response.ReponseStatus == ServiceResponseStatuses.Sussess)
			{
				return Ok("Блюдо успешно создано");
			}

			if (response.ReponseStatus == ServiceResponseStatuses.ValidationError)
			{
				return UnprocessableEntity(response.Message);
			}

			return BadRequest(response.Message);
		}

		/// <summary>
		/// Обнолвение блюда
		/// </summary>
		/// <param name="updateDishRequest"></param>
		/// <returns></returns>
		[HttpPut("Update")]
		[Authorize(Roles = ApplicationRoles.Manager)]
		public async Task<ActionResult> Update([FromBody] UpdateDishRequest updateDishRequest)
		{
			var response = await _dishService.UpdateDishAsync(updateDishRequest);

			if (response.ReponseStatus == ServiceResponseStatuses.Sussess)
			{
				return Ok("Блюдо успешно обновлено");
			}

			if (response.ReponseStatus == ServiceResponseStatuses.ValidationError)
			{
				return UnprocessableEntity(response.Message);
			}

			return BadRequest(response.Message);
		}

		/// <summary>
		/// Удаление блюда по Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		[Authorize(Roles = ApplicationRoles.Manager)]
		public async Task<ActionResult> DeleteById(int id)
		{
			var response = await _dishService.DeleteDishAsync(id);

			if (response.ReponseStatus == ServiceResponseStatuses.Sussess)
			{
				return Ok("Блюдо успешно удалено");
			}

			if (response.ReponseStatus == ServiceResponseStatuses.ValidationError)
			{
				return UnprocessableEntity(response.Message);
			}

			return BadRequest(response.Message);
		}

		/// <summary>
		/// Получение меню
		/// </summary>
		/// <returns></returns>
		[HttpGet("Menu")]
		public async Task<ActionResult> GetMenu()
		{
			return Ok(await _dishService.GetMenuAsync());
		}
	}
}
