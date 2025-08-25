using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace campuslove_Juliana_Eduardo.src.Shared.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
              public void Configure(EntityTypeBuilder<Usuario> builder)
              {
                     builder.ToTable("usuario");

                     builder.HasKey(u => u.Id);

                     builder.Property(u => u.Nombre)
                            .IsRequired()
                            .HasMaxLength(100);

                     builder.Property(u => u.Clave)
                          .IsRequired()
                          .HasMaxLength(100);

                     

              
        }
    }
}
