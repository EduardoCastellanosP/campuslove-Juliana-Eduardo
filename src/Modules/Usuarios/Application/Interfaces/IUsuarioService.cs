using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task CrearUsuarioAsync(string nombreusuario,string clave); 
        Task RegistrarUsuarioAsync(string nombre, string clave );
        
        Task EliminarUsuarioAsync(int id);
        Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);
        Task<IEnumerable<Usuario>> ConsultarUsuarioAsync();
    }
}