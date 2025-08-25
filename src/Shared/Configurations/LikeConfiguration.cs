using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;

namespace campuslove_Juliana_Eduardo.src.Shared.Configurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("likes");
            builder.HasKey(l => l.Id);

            builder.Property(l => l.UsuarioId).IsRequired();
            builder.Property(l => l.UsuarioLikedId).IsRequired();
            builder.Property(l => l.Fecha).HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(l => l.Usuario)
                   .WithMany()
                   .HasForeignKey(l => l.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.UsuarioLiked)
                   .WithMany()
                   .HasForeignKey(l => l.UsuarioLikedId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(l => new { l.UsuarioId, l.UsuarioLikedId }).IsUnique();
        }
    }
}
