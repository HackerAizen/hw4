using OrderMicroservice.DTO;

namespace OrderMicroservice.Models.Responses
{
	public class MenuResponse: ServiceResponse
	{
		public DishResponse[] Dishes { get; set; }
	}
}
