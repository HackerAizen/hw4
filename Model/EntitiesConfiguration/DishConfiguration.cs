using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntitiesConfiguration
{
	internal class DishConfiguration : IEntityTypeConfiguration<Dish>
	{
		public void Configure(EntityTypeBuilder<Dish> builder)
		{
			builder.ToTable("dish");

			builder
				.HasKey(x => x.Id);

			builder
				.Property(x => x.Id)
				.HasColumnName("id")
				.HasColumnType("int")
				.ValueGeneratedOnAdd();

			builder
				.Property(x => x.Name)
				.HasColumnName("name")
				.HasColumnType("varchar")
				.IsRequired();

			builder
				.Property(x => x.Description)
				.HasColumnName("description")
				.HasColumnType("text");

			builder
				.Property(x => x.Price)
				.HasColumnName("price")
				.HasColumnType("decimal(10,2)")
				.IsRequired();


			builder
				.Property(x => x.Quantity)
				.HasColumnName("quantity")
				.HasColumnType("int")
				.IsRequired();

			builder
				.Property(x =>x.IsAvailable)
				.HasColumnName("is_available")
				.HasColumnType("boolean")
				.IsRequired();
		}
	}
}
