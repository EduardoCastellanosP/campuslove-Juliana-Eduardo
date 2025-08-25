using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task CrearUsuarioAsync(string nombre,string clave); 
        Task RegistrarUsuarioAsync(string nombre,string email, int edad,  string genero, string profesion, string intereses, string frase );
        Task ActualizarUsuarioAsync(int id, string nuevoNombre,string nuevoEmail, int nuevaEdad,  string nuevoGenero, string nuevaProfesion, string nuevoIntereses, string nuevaFrase );
        Task EliminarUsuarioAsync(int id);
        Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);
        Task<IEnumerable<Usuario>> ConsultarUsuarioAsync();
    }
}