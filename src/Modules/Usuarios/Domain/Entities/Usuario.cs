using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities
{
    public class Usuario
    {
       public int Id { get; set; }
       public string? Nombre { get; set; }

       public string? Email { get; set; }

        public int Edad { get; set; }

        public string? Genero { get; set; }

       public string? Profesion { get; set; }

      public ICollection<Interes> Intereses { get; set; } = new List<Interes>();
        
        public string? Frase { get; set; }
    }
}