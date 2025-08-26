using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities
{
    public class Dato
    {

        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Edad { get; set; }
        public string Genero { get; set; } = null!;
        public string Profesion { get; set; } = null!;
        public string Frase { get; set; } = null!;

        public string Intereses { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    
       


       
    }
}