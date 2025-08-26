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
            builder.Property(l => l.Id).HasColumnName("id");

            builder.Property(l => l.UsuarioId).HasColumnName("usuario_id").IsRequired();
            builder.Property(l => l.UsuarioLikedId).HasColumnName("usuario_liked_id").IsRequired();

        
            builder.Property(l => l.EsLike)
                   .HasColumnName("es_like")
                   .HasDefaultValue(true)   
                   .IsRequired();

            builder.Property(l => l.Fecha)
                   .HasColumnName("fecha")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            
            builder.HasOne(l => l.Usuario)
                   .WithMany(u => u.LikesDados)
                   .HasForeignKey(l => l.UsuarioId)
                   .HasConstraintName("fk_likes_usuario")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.UsuarioLiked)
                   .WithMany(u => u.LikesRecibidos)
                   .HasForeignKey(l => l.UsuarioLikedId)
                   .HasConstraintName("fk_likes_usuario_liked")
                   .OnDelete(DeleteBehavior.Cascade);

            
            builder.HasIndex(l => new { l.UsuarioId, l.UsuarioLikedId, l.EsLike })
                   .IsUnique()
                   .HasDatabaseName("ux_likes_pair_type");
        }
    }
}
