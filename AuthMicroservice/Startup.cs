using AuthMicroservice.Configuration;
using AuthMicroservice.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Model;
using System.Text.Json.Serialization;

namespace AuthMicroservice
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

			services.Configure<JwtConfiguration>(_configuration.GetSection("JwtConfiguration"));


			services.AddDbContextFactory<OrdersProcessingDbContext>(options => options
						.UseLazyLoadingProxies()
						.UseNpgsql(_configuration.GetConnectionString("PostgreSql")));

			services.AddTransient<IAuthService, AuthService>();
			services.AddControllers();

			services.AddSwaggerGen(o =>
			{
				o.SwaggerDoc("v1",
					new OpenApiInfo
					{
						Title = "API Авторизации",
						Description = "",
						Version = "v1",
						TermsOfService = null,
					});
				var filePath = Path.Combine(AppContext.BaseDirectory, "AuthMicroservice.xml");
				o.IncludeXmlComments(filePath);
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseCors("CorsPolicy");

			app.UseHttpsRedirection();

			app.UseRouting();

			//app.UseAuthentication();
			//app.UseAuthorization();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
