using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Model;
using OrderMicroservice.Configuration;
using OrderMicroservice.Middleware;
using OrderMicroservice.Services;
using System.Text.Json.Serialization;

namespace OrderMicroservice
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

			services.AddSwaggerGen();

			services.AddDbContextFactory<OrdersProcessingDbContext>(options => options
						.UseLazyLoadingProxies()
						.UseNpgsql(_configuration.GetConnectionString("PostgreSql")));

			services.AddTransient<IOrderService, OrderService>();
			services.AddTransient<IDishService, DishService>();
			services.AddControllers();

			services.AddSwaggerGen(o =>
			{
				o.SwaggerDoc("v1",
					new OpenApiInfo
					{
						Title = "API Заказов",
						Description = "API для управления заказами и блюдами",
						Version = "v1",
						TermsOfService = null,
					});
				var filePath = Path.Combine(AppContext.BaseDirectory, "OrderMicroservice.xml");
				o.IncludeXmlComments(filePath);

				o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				o.AddSecurityRequirement(new OpenApiSecurityRequirement()
				  {
					{
					  new OpenApiSecurityScheme
					  {
						Reference = new OpenApiReference
						  {
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						  },
						  Scheme = "oauth2",
						  Name = "Bearer",
						  In = ParameterLocation.Header,

						},
						new List<string>()
					  }
					});
			});

			IConfigurationSection authConfigSection = _configuration.GetSection("AuthConfiguration");
			AuthConfiguration authConfiguration = authConfigSection.Get<AuthConfiguration>();

			services.Configure<AuthConfiguration>(authConfigSection);

			services.AddHttpClient(
			   "AuthClient",
			   client =>
			   {
				   client.BaseAddress = new Uri(authConfiguration.Uri);
			   });

			services.AddTransient<IAuthService, AuthService>();

			services.AddAuthentication("Bearer")
				.AddScheme<AuthenticationSchemeOptions, AuthMiddleware>("Bearer", null);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			app.UseExceptionHandler(b => ErrorHandlerMiddleware.Handle(b));

			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseCors("CorsPolicy");

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();



			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
