using System.ComponentModel.DataAnnotations;

namespace OrderMicroservice.Models.Requests
{
	/// <summary>
	/// Модель создания блюда
	/// </summary>
	public class CreateDishRequest
	{
		/// <summary>
		/// Название блюда
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage ="Необходимо ввести название блюда")]
		public string Name { get; set; }
		
		/// <summary>
		/// Описание блюда
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Цена блюда
		/// </summary>
		[Range(0, 1_000_000)]
		public decimal Price { get; set; }

		/// <summary>
		/// Количество штук блюда в наличии
		/// </summary>
		[Range(0, 1_000_000)]
		public int Quantity { get; set; }
	}
}
