using System;
using System.Linq; // <-- IMPORTANTE
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using campuslove_Juliana_Eduardo.src.Shared.Context;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;

namespace campuslove_Juliana_Eduardo.src.Modules.Perfiles.UI
{
    public class Perfil
    {
        private readonly AppDbContext _context;

        public Perfil(AppDbContext context)
        {
            _context = context;
        }

        public async Task VerPerfilesAsync(int miUsuarioId)
        {
            if (miUsuarioId <= 0)
            {
                Console.WriteLine("⚠️ Debes iniciar sesión (miUsuarioId inválido).");
                Console.ReadKey(true);
                return;
            }

            // Cargar usuarios + su perfil (tabla 'datos')
            var usuarios = await _context.Set<Usuario>()
                .AsNoTracking()
                .Include(u => u.Dato) 
                .Where(u => u.Id != miUsuarioId) // no mostrarte a ti mismo
                .ToListAsync();

            if (usuarios.Count == 0)
            {
                Console.WriteLine("⚠️ No hay otros usuarios para mostrar.");
                Console.ReadKey(true);
                return;
            }

            int idx = 0;
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                var u = usuarios[idx];
                var d = u.Dato; // puede ser null si aún no registraron "datos" para ese usuario

                Console.WriteLine("=== Perfil ===");
                Console.WriteLine($"ID        : {u.Id}");
                Console.WriteLine($"Nombre    : {d?.Nombre ?? u.Nombre}"); // muestra el de datos si existe, sino el de usuario

                // Campos que ahora están en 'datos'
                Console.WriteLine($"Edad      : {(d is null ? "—" : d.Edad.ToString())}");
                Console.WriteLine($"Género    : {d?.Genero ?? "—"}");
                Console.WriteLine($"Profesión : {d?.Profesion ?? "—"}");
                Console.WriteLine($"Intereses : {d?.Intereses ?? "—"}");
                Console.WriteLine($"Frase     : {d?.Frase ?? "—"}");

                var likes = await ContarLikesRecibidosAsync(u.Id);
                Console.WriteLine($"👍 Likes  : {likes}");
                Console.WriteLine(new string('-', 40));

                Console.WriteLine("[N] Siguiente  [P] Anterior  [L] Like  [D] Dislike  [Q] Salir");
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.N:
                        idx = (idx + 1) % usuarios.Count;
                        break;

                    case ConsoleKey.P:
                        idx = (idx - 1 + usuarios.Count) % usuarios.Count;
                        break;

                    case ConsoleKey.L:
                        try
                        {
                            await DarLikeAsync(miUsuarioId, u.Id);
                            var nuevosLikes = await ContarLikesRecibidosAsync(u.Id);
                            Console.WriteLine($"❤️ Diste like a {d?.Nombre ?? u.Nombre}. Ahora tiene {nuevosLikes} likes.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"⚠️ {ex.Message}");
                        }
                        Console.WriteLine("Presiona una tecla para continuar...");
                        Console.ReadKey(true);
                        idx = (idx + 1) % usuarios.Count; 
                        break;

                    case ConsoleKey.D:
                        Console.WriteLine($"👎 Dislike a {d?.Nombre ?? u.Nombre}.");
                        Console.WriteLine("Presiona una tecla para continuar...");
                        Console.ReadKey(true);
                        idx = (idx + 1) % usuarios.Count;
                        break;

                    case ConsoleKey.Q:
                        salir = true;
                        break;
                }
            }
        }

        private Task<int> ContarLikesRecibidosAsync(int usuarioId)
        {
            return _context.Likes.CountAsync(l => l.UsuarioLikedId == usuarioId);
        }

        private async Task DarLikeAsync(int usuarioId, int usuarioLikedId)
        {
            if (usuarioId == usuarioLikedId)
                throw new Exception("No puedes darte like a ti mismo.");

            bool yaExiste = await _context.Likes
                .AnyAsync(l => l.UsuarioId == usuarioId && l.UsuarioLikedId == usuarioLikedId);

            if (yaExiste)
                throw new Exception("Ya diste like a este usuario.");

            await _context.Likes.AddAsync(new Like
            {
                UsuarioId = usuarioId,
                UsuarioLikedId = usuarioLikedId,
                Fecha = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }
    }
}
