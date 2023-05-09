using OrderMicroservice.DTO;
using OrderMicroservice.Models.Requests;
using OrderMicroservice.Models.Responses;

namespace OrderMicroservice.Services
{
	public interface IOrderService
	{
		Task<ServiceResponse> CreateOrder(CreateOrderRequest createOrderRequest);
		Task<OrderInfoResponse> GetOrderInfo(int id);
	}
}
