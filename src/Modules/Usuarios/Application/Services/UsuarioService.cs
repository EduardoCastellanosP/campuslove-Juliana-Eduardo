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

        public async Task CrearUsuarioAsync(string nombreusuario, string clave)
        {
            var existentes = await _repo.GetAllAsync();

            if (existentes.Any(u => u.Nombre == nombreusuario))
                throw new Exception("El usuario ya existe.");

            var usuario = new Usuario
            {
                Nombre = nombreusuario,
                Clave = clave

            };

            await _repo.Add(usuario);
            await _repo.SaveAsync();
        }

       public async Task RegistrarUsuarioAsync(string nombre, string clave)
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new Exception("Nombre requerido.");
                if (string.IsNullOrWhiteSpace(clave))
                    throw new Exception("Clave requerida.");

                var nombreNorm = nombre.Trim();
                var claveNorm  = clave.Trim();

                // Evitar duplicados por nombre
                var existente = await _repo.GetByNombreAsync(nombreNorm);
                if (existente != null)
                    throw new Exception("El usuario ya existe.");

                var usuario = new Usuario
                {
                    Nombre = nombreNorm,
                    Clave  = claveNorm,
                    LikesDisponibles = 5
                };

                await _repo.Add(usuario);
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