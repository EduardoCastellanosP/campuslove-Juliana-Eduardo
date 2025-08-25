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

            builder.Property(d => d.Nombre)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.Email)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasIndex(d => d.Email).IsUnique();

            builder.Property(d => d.Edad)
                   .IsRequired();

            builder.Property(d => d.Genero)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.Profesion)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.Frase)
                   .IsRequired()
                   .HasMaxLength(100);

            // Relación 1–1 con Usuario
            builder.HasOne(d => d.Usuario)
                   .WithOne(u => u.Dato) 
                   .HasForeignKey<Dato>(d => d.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
