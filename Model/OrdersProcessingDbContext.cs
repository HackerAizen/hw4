using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Model.EntitiesConfiguration;

namespace Model
{
	public class OrdersProcessingDbContext : DbContext
	{
		public OrdersProcessingDbContext(DbContextOptions<OrdersProcessingDbContext> options)
		   : base(options)
		{

		}

		public DbSet<User> Users { get; set; }
		public DbSet<Session> Sessions { get; set; }
		public DbSet<Dish> Dishes { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDish> OrderDishes { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfiguration(new UserConfiguration());
			builder.ApplyConfiguration(new SessionConfiguration());
			builder.ApplyConfiguration(new DishConfiguration());
			builder.ApplyConfiguration(new OrderConfiguration());
			builder.ApplyConfiguration(new OrderDishConfiguration());

		}

	}
}
