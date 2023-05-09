using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntitiesConfiguration
{
	internal class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.ToTable("order");

			builder
				.HasKey(x => x.Id);

			builder
				.Property(x => x.Id)
				.HasColumnName("id")
				.HasColumnType("int")
				.ValueGeneratedOnAdd();

			builder
				.Property(x => x.UserId)
				.HasColumnName("user_id")
				.HasColumnType("int")
				.IsRequired();

			builder
				.Property(x => x.Status)
				.HasColumnName("status")
				.HasColumnType("varchar(50)")
				.IsRequired();

			builder
				.Property(x => x.SpecialRequests)
				.HasColumnName("special_requests")
				.HasColumnType("text");

			builder
				.Property(x => x.Created)
				.HasColumnName("created_at")
				.IsRequired();

			builder
				.Property(x => x.Updated)
				.HasColumnName("updated_at")
				.IsRequired();

			builder
				.HasOne(x => x.User)
				.WithMany(x => x.Orders)
				.HasForeignKey(x => x.UserId)
				.IsRequired();

		}
	}
}
