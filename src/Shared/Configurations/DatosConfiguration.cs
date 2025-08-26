using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;

namespace campuslove_Juliana_Eduardo.src.Shared.Configurations
{
    public class DatosConfiguration : IEntityTypeConfiguration<Dato>
    {
              public void Configure(EntityTypeBuilder<Dato> builder)
                     {
                     builder.ToTable("datos");

                     builder.HasKey(d => d.UsuarioId);
                     builder.Property(d => d.UsuarioId).HasColumnName("usuario_id");

                     builder.Property(d => d.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
                     builder.Property(d => d.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
                     builder.HasIndex(d => d.Email).IsUnique();

                     builder.Property(d => d.Edad).HasColumnName("edad").IsRequired();
                     builder.Property(d => d.Genero).HasColumnName("genero").HasMaxLength(100).IsRequired();
                     builder.Property(d => d.Profesion).HasColumnName("profesion").HasMaxLength(100).IsRequired();
                     builder.Property(d => d.Intereses).HasColumnName("intereses").HasMaxLength(100).IsRequired();
                     builder.Property(d => d.Frase).HasColumnName("frase").HasMaxLength(100).IsRequired();

                     builder.HasOne(d => d.Usuario)
                            .WithOne(u => u.Dato)
                            .HasForeignKey<Dato>(d => d.UsuarioId)
                            .OnDelete(DeleteBehavior.Cascade);
}
    }
}
