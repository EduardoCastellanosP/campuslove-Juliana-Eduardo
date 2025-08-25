using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities
{
    public class Like
    {
         public int Id { get; set; }

        public int UsuarioId { get; set; }      
        public int UsuarioLikedId { get; set; } 

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public Usuario? Usuario { get; set; }
        public Usuario? UsuarioLiked { get; set; }
    }
}