using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces
{
    public interface IDatoService
    {
        Task RegistrarDatosAsync(
       int usuarioId,
       string nombre,
       string email,
       int edad,
       string genero,
       string profesion,
       string intereses,
       string frase);
        
        Task ActualizarDatosAsync(
            int usuarioId,
            string nuevoNombre,
            string nuevoEmail,
            int nuevaEdad,
            string nuevoGenero,
            string nuevaProfesion,
            string nuevoIntereses,
            string nuevaFrase);
    }
}