using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntitiesConfiguration
{
	internal class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("user");

			builder
				.HasKey(x => x.Id);

			builder
				.Property(x => x.Id)
				.HasColumnName("id")
				.HasColumnType("int")
				.ValueGeneratedOnAdd();

			builder
				.Property(x => x.Email)
				.HasColumnName("email")
				.HasColumnType("varchar(100)")
				.IsRequired();

			builder.
				HasIndex(u => u.Email)
				.IsUnique();

			builder
				.Property(x => x.UserName)
				.HasColumnName("username")
				.HasColumnType("varchar(50)")
				.IsRequired();

			builder.
				HasIndex(u => u.UserName)
				.IsUnique();

			builder
				.Property(x => x.PasswordHash)
				.HasColumnName("password_hash")
				.HasColumnType("varchar(255)")
				.IsRequired();

			builder
				.Property(x => x.Role)
				.HasColumnName("role")
				.HasColumnType("varchar(10)")
				.IsRequired();

			builder
				.Property(x => x.Created)
				.HasColumnName("created_at")
				.IsRequired();

			builder
				.Property(x => x.Updated)
				.HasColumnName("updated_at")
				.IsRequired();
		}
	}
}
