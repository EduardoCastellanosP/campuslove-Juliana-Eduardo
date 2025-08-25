using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Clave  { get; set; } = null!;
    public int LikesDisponibles { get; set; } = 5;

    public Dato Dato { get; set; } = null!;
        


        
    }
}