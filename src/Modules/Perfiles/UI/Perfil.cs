// src/Modules/Perfiles/UI/Perfil.cs
using System;
using System.Linq; // necesario para Where/Any/SingleOrDefault
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

            var usuarios = await _context.Set<Usuario>()
                .AsNoTracking()
                .Include(u => u.Dato)
                .Where(u => u.Id != miUsuarioId)
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

                var nombre     = d?.Nombre ?? u.Nombre;
                var edad       = d != null ? d.Edad.ToString() : "—";
                var genero     = string.IsNullOrWhiteSpace(d?.Genero)     ? "—" : d!.Genero;
                var profesion  = string.IsNullOrWhiteSpace(d?.Profesion)  ? "—" : d!.Profesion;
                var intereses  = string.IsNullOrWhiteSpace(d?.Intereses)  ? "—" : d!.Intereses;
                var frase      = string.IsNullOrWhiteSpace(d?.Frase)      ? "—" : d!.Frase;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.ResetColor();

                Console.WriteLine($"📇  Perfil de {nombre}\n");
                Console.WriteLine($"{"ID".PadRight(12)}: {u.Id}");
                Console.WriteLine($"{"Nombre".PadRight(12)}: {nombre}");
                Console.WriteLine($"{"Edad".PadRight(12)}: {edad}");
                Console.WriteLine($"{"Género".PadRight(12)}: {genero}");
                Console.WriteLine($"{"Profesión".PadRight(12)}: {profesion}");
                Console.WriteLine($"{"Intereses".PadRight(12)}: {intereses}");
                Console.WriteLine($"{"Frase".PadRight(12)}: {frase}");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.ResetColor();

                var likes    = await ContarLikesRecibidosAsync(u.Id);
                var dislikes = await ContarDisLikesRecibidosAsync(u.Id);
                Console.WriteLine($"👍 Likes  : {likes}    👎 Dislikes : {dislikes}");
                Console.WriteLine(new string('-', 42));

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
                    {
                        var creado = await DarLikeAsync(miUsuarioId, u.Id);
                        if (creado)
                        {
                            var nuevosLikes = await ContarLikesRecibidosAsync(u.Id);
                            Console.WriteLine($"❤️ Diste like a {d?.Nombre ?? u.Nombre}. Ahora tiene {nuevosLikes} likes.");
                        }
                        Console.WriteLine("Presiona una tecla para continuar...");
                        Console.ReadKey(true);
                        idx = (idx + 1) % usuarios.Count;
                        break;
                    }

                    case ConsoleKey.D:
                    {
                        var creado = await DarDisLikeAsync(miUsuarioId, u.Id);
                        if (creado)
                        {
                            var nuevosDislikes = await ContarDisLikesRecibidosAsync(u.Id);
                            Console.WriteLine($"👎 Dislike a {d?.Nombre ?? u.Nombre}. Ahora tiene {nuevosDislikes} dislikes.");
                        }
                        Console.WriteLine("Presiona una tecla para continuar...");
                        Console.ReadKey(true);
                        idx = (idx + 1) % usuarios.Count;
                        break;
                    }

                    case ConsoleKey.Q:
                        salir = true;
                        break;
                }
            }
        }

        private Task<int> ContarLikesRecibidosAsync(int usuarioId) =>
            _context.Likes.CountAsync(l => l.UsuarioLikedId == usuarioId && l.EsLike);

        private Task<int> ContarDisLikesRecibidosAsync(int usuarioId) =>
            _context.Likes.CountAsync(l => l.UsuarioLikedId == usuarioId && !l.EsLike);

        private async Task<bool> ReaccionarAsync(int usuarioId, int usuarioLikedId, bool esLike)
        {
            if (usuarioId == usuarioLikedId)
            {
                Console.WriteLine("⚠️ No puedes reaccionarte a ti mismo.");
                return false;
            }

           
            bool yaExiste = await _context.Likes
                .AnyAsync(l => l.UsuarioId == usuarioId && l.UsuarioLikedId == usuarioLikedId);

            if (yaExiste)
            {
                Console.WriteLine("ℹ️ Ya reaccionaste a este usuario (solo se permite 1 reacción total).");
                return false;
            }

            var actor = await _context.Usuarios
                .SingleOrDefaultAsync(u => u.Id == usuarioId);

            if (actor is null)
            {
                Console.WriteLine("❌ Usuario no encontrado.");
                return false;
            }

            if (actor.LikesDisponibles <= 0)
            {
                Console.WriteLine("⚠️ No te quedan reacciones disponibles.");
                return false;
            }

            
            await _context.Likes.AddAsync(new Like
            {
                UsuarioId = usuarioId,
                UsuarioLikedId = usuarioLikedId,
                EsLike = esLike,
                Fecha = DateTime.UtcNow
            });

            actor.LikesDisponibles -= 1;

            await _context.SaveChangesAsync();

            Console.WriteLine($"🔋 Te quedan {actor.LikesDisponibles} reacciones.");
            return true;
        }

        private Task<bool> DarLikeAsync(int usuarioId, int usuarioLikedId) =>
            ReaccionarAsync(usuarioId, usuarioLikedId, true);

        private Task<bool> DarDisLikeAsync(int usuarioId, int usuarioLikedId) =>
            ReaccionarAsync(usuarioId, usuarioLikedId, false);
    }
}
