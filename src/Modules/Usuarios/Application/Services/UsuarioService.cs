using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;



        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;

        }

        public Task<IEnumerable<Usuario>> ConsultarUsuarioAsync()
        {
            return _repo.GetAllAsync()!;
        }

        public async Task CrearUsuarioAsync(string nombre, string clave)
        {
            var existentes = await _repo.GetAllAsync();

            if (existentes.Any(u => u.Nombre == nombre))
                throw new Exception("El usuario ya existe.");

            var usuario = new Usuario
            {
                Nombre = nombre,
                Clave = clave

            };

            await _repo.Add(usuario);
            await _repo.SaveAsync();
        }

        public async Task RegistrarUsuarioAsync(string nombre, string email, int edad, string genero, string profesion, string intereses, string frase)
        {
            var existentes = await _repo.GetAllAsync();
            if (existentes.Any(j => j?.Email == email))
                throw new Exception("⚠️ El Email ya está registrado.");

            var usuario = new Usuario
            {
                Nombre = nombre,
                Email = email,
                Edad = edad,
                Genero = genero,
                Profesion = profesion,
                Intereses = intereses,
                Frase = frase
            };

            await _repo.Add(usuario);
            await _repo.SaveAsync();
        }

        public async Task ActualizarUsuarioAsync(int id, string? nuevoNombre, string nuevoEmail, int nuevaEdad, string nuevoGenero, string nuevaProfesion, string nuevoIntereses, string nuevaFrase)
        {
            var usuario = await _repo.GetByIdAsync(id);

            if (usuario == null)
                throw new Exception($"❌ Usuario con ID {id} no encontrado.");

            usuario.Nombre = nuevoNombre;
            usuario.Email = nuevoEmail;
            usuario.Edad = nuevaEdad;
            usuario.Genero = nuevoGenero;
            usuario.Profesion = nuevaProfesion;
            usuario.Intereses = nuevoIntereses;
            usuario.Frase = nuevaFrase;

            await _repo.Update(usuario);
            await _repo.SaveAsync();
        }

        public async Task EliminarUsuarioAsync(int id)
        {
            var usuario = await _repo.GetByIdAsync(id);

            if (usuario == null)
                throw new Exception($"❌ Usuario con ID {id} no encontrado.");

            await _repo.Remove(usuario);
            await _repo.SaveAsync();
        }

        public async Task<Usuario> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
        
         public async Task<Usuario?> ObtenerUsuarioPorNombreAsync(string nombre)
        {
            return await _repo.GetByNombreAsync(nombre);
        }
    
   

        
    }
}