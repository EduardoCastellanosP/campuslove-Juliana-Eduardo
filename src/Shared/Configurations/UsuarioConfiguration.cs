using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasIndex(u => u.Email).IsUnique();;

            builder.Property(j => j.Edad)
         .IsRequired();

            builder.Property(u => u.Genero)
                      .IsRequired()
                      .HasMaxLength(100);

            builder.Property(u => u.Profesion)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Intereses)
                   .IsRequired()
                   .HasMaxLength(100);
                   
            builder.Property(u => u.Frase)
                   .IsRequired()
                   .HasMaxLength(200);


            

        }
        
    }
}