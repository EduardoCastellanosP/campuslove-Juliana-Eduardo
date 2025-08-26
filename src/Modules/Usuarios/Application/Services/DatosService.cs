// src/Modules/Usuarios/Application/Services/DatosService.cs
using System;
using System.Threading.Tasks;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;

public class DatosService : IDatoService
{
    private readonly IDatoRepository _datosRepo;
    private readonly IUsuarioRepository _usuarioRepo;

    public DatosService(IDatoRepository datosRepo, IUsuarioRepository usuarioRepo)
    {
        _datosRepo = datosRepo ?? throw new ArgumentNullException(nameof(datosRepo));
        _usuarioRepo = usuarioRepo ?? throw new ArgumentNullException(nameof(usuarioRepo));
    }

    
    // DatosService.cs (método para registrar DESDE CERO)
        public async Task RegistrarDatosAsync(
            int usuarioId, string nombre, string email, int edad,
            string genero, string profesion, string intereses, string frase)
        {
            // FK válida
            var user = await _usuarioRepo.GetByIdAsync(usuarioId)
                    ?? throw new Exception("Usuario no existe. Crea primero usuario (nombre/clave).");

            // Ya tiene perfil?
            var yaTiene = await _datosRepo.GetByUsuarioIdAsync(usuarioId);
            if (yaTiene != null)
                throw new Exception("Este usuario ya tiene datos registrados. Usa 'Actualizar datos'.");

            // Email único
            var emailNorm = (email ?? "").Trim().ToLowerInvariant();
            if (await _datosRepo.ExistsEmailAsync(emailNorm))
                throw new Exception("El email ya está registrado.");

            var dato = new Dato
            {
                UsuarioId = usuarioId,
                Nombre    = (nombre ?? "").Trim(),
                Email     = emailNorm,
                Edad      = edad,
                Genero    = (genero ?? "").Trim(),
                Profesion = (profesion ?? "").Trim(),
                Intereses = (intereses ?? "").Trim(),
                Frase     = (frase ?? "").Trim()
            };

            await _datosRepo.Add(dato);
            await _datosRepo.SaveAsync();
        }


       public async Task ActualizarDatosAsync(
        int usuarioId,
        string nuevoNombre,
        string nuevoEmail,
        int nuevaEdad,
        string nuevoGenero,
        string nuevaProfesion,
        string nuevoIntereses,
        string nuevaFrase)
    {
  
        var usuario = await _usuarioRepo.GetByIdAsync(usuarioId)
                     ?? throw new Exception($"❌ Usuario con ID {usuarioId} no encontrado.");

      
        if (!string.IsNullOrWhiteSpace(nuevoNombre))
        {
            var n = nuevoNombre.Trim();
            if (usuario.Nombre != n)
            {
                usuario.Nombre = n;
                await _usuarioRepo.Update(usuario);
            }
        }

        var dato = await _datosRepo.GetByUsuarioIdAsync(usuarioId);
        if (dato is null)
        {
            dato = new Dato { UsuarioId = usuarioId };
            await _datosRepo.Add(dato);
        }

        var emailNorm = (nuevoEmail ?? string.Empty).Trim().ToLowerInvariant();
        if (!string.IsNullOrEmpty(emailNorm) &&
            !string.Equals(dato.Email ?? string.Empty, emailNorm, StringComparison.OrdinalIgnoreCase))
        {
            if (await _datosRepo.ExistsEmailAsync(emailNorm))
                throw new Exception("⚠️ El email ya está registrado.");
            dato.Email = emailNorm;
        }

        
        dato.Nombre    = string.IsNullOrWhiteSpace(nuevoNombre) ? dato.Nombre : nuevoNombre.Trim();
        dato.Edad      = nuevaEdad;
        dato.Genero    = (nuevoGenero ?? string.Empty).Trim();
        dato.Profesion = (nuevaProfesion ?? string.Empty).Trim();
        dato.Intereses = (nuevoIntereses ?? string.Empty).Trim();
        dato.Frase     = (nuevaFrase ?? string.Empty).Trim();

        await _datosRepo.Update(dato);

        await _usuarioRepo.SaveAsync();
        await _datosRepo.SaveAsync();
    }
}
