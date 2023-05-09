using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntitiesConfiguration
{
	internal class OrderDishConfiguration : IEntityTypeConfiguration<OrderDish>
	{
		public void Configure(EntityTypeBuilder<OrderDish> builder)
		{
			builder.ToTable("order_dish");

			builder
				.HasKey(x => x.Id);

			builder
				.Property(x => x.Id)
				.HasColumnName("id")
				.HasColumnType("int")
				.ValueGeneratedOnAdd();

			builder
				.Property(x => x.OrderId)
				.HasColumnName("order_id")
				.HasColumnType("int")
				.IsRequired();

			builder
				.Property(x => x.DishId)
				.HasColumnName("dish_id")
				.HasColumnType("int")
				.IsRequired();

			builder
				.Property(x => x.Quantity)
				.HasColumnName("quantity")
				.HasColumnType("int")
				.IsRequired();

			builder
				.Property(x => x.Price)
				.HasColumnType("decimal(10,2)")
				.HasColumnName("price")
				.IsRequired();


			builder
				.HasOne(x => x.Order)
				.WithMany(x => x.OrderDishes)
				.HasForeignKey(x => x.OrderId)
				.IsRequired();

			builder
				.HasOne(x => x.Dish)
				.WithMany(x => x.OrderDishes)
				.HasForeignKey(x => x.DishId)
				.IsRequired();
		}
	}
}
