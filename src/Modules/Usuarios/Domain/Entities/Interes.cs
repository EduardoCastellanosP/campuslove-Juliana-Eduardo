using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities
{
    public class Interes
    {
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;

    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    }
}