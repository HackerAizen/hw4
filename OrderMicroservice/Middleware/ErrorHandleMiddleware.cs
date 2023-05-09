using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using System.Text;

namespace OrderMicroservice.Middleware
{
	internal static class ErrorHandlerMiddleware
	{
		public static void Handle(IApplicationBuilder applicationBuilder)
		{
			applicationBuilder.Run(async context =>
			{
				var bytes = Encoding.UTF8.GetBytes("Внутренняя ошибка приложения");
				await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
				context.Response.StatusCode = 500;
			});
		}
	}
}
