using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntitiesConfiguration
{
	internal class SessionConfiguration : IEntityTypeConfiguration<Session>
	{
		public void Configure(EntityTypeBuilder<Session> builder)
		{
			builder.ToTable("session");

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
				.Property(x => x.ExpiresAt)
				.HasColumnName("expires_at")
				.IsRequired();

			builder
				.Property(x => x.JwtToken)
				.HasColumnName("session_token")
				.HasColumnType("varchar")
				.IsRequired();

			builder
				.HasOne(x => x.User)
				.WithMany(x => x.Sessions)
				.HasForeignKey(x => x.UserId);
		}
	}
}
