using OrderMicroservice.DTO;
using OrderMicroservice.Models.Requests;
using OrderMicroservice.Models.Responses;

namespace OrderMicroservice.Services
{
	public interface IDishService
	{
		Task<ServiceResponse> CreateDishAsync(CreateDishRequest createDish);
		Task<DishResponse> GetDishInfoAsync(int id);
		Task<ServiceResponse> DeleteDishAsync(int id);
		Task<ServiceResponse> UpdateDishAsync(UpdateDishRequest updateDishRequest);
		Task<MenuResponse> GetMenuAsync();
	}
}
