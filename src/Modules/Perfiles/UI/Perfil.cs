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
                var d = u.Dato;

                var nombre = d?.Nombre ?? u.Nombre;
                var edad = d != null ? d.Edad.ToString() : "—";
                var genero = string.IsNullOrWhiteSpace(d?.Genero) ? "—" : d!.Genero;
                var profesion = string.IsNullOrWhiteSpace(d?.Profesion) ? "—" : d!.Profesion;
                var intereses = string.IsNullOrWhiteSpace(d?.Intereses) ? "—" : d!.Intereses;
                var frase = string.IsNullOrWhiteSpace(d?.Frase) ? "—" : d!.Frase;

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

                var likes = await ContarLikesRecibidosAsync(u.Id);
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
                                Console.WriteLine($"❤️ Diste like a {nombre}. Ahora tiene {nuevosLikes} likes.");
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
                                Console.WriteLine($"👎 Dislike a {nombre}. Ahora tiene {nuevosDislikes} dislikes.");
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

        public async Task VerMatchesAsync(int miUsuarioId)
        {
            if (miUsuarioId <= 0)
            {
                Console.WriteLine("⚠️ Debes iniciar sesión (miUsuarioId inválido).");
                Console.ReadKey(true);
                return;
            }

            var mutualIds = await _context.Likes
                .Where(l => l.UsuarioId == miUsuarioId && l.EsLike)
                .Select(l => l.UsuarioLikedId)
                .Where(otroId => _context.Likes.Any(l2 =>
                    l2.UsuarioId == otroId &&
                    l2.UsuarioLikedId == miUsuarioId &&
                    l2.EsLike))
                .Distinct()
                .ToListAsync();

            if (mutualIds.Count == 0)
            {
                Console.WriteLine("🫤 Aún no tienes matches. ¡Sigue explorando perfiles!");
                Console.ReadKey(true);
                return;
            }

            var usuarios = await _context.Usuarios
                .AsNoTracking()
                .Include(u => u.Dato)
                .Where(u => mutualIds.Contains(u.Id))
                .ToListAsync();

            int idx = 0;
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                var u = usuarios[idx];
                var d = u.Dato;

                var nombre = d?.Nombre ?? u.Nombre;
                var edad = d != null ? d.Edad.ToString() : "—";
                var genero = string.IsNullOrWhiteSpace(d?.Genero) ? "—" : d!.Genero;
                var profesion = string.IsNullOrWhiteSpace(d?.Profesion) ? "—" : d!.Profesion;
                var intereses = string.IsNullOrWhiteSpace(d?.Intereses) ? "—" : d!.Intereses;
                var frase = string.IsNullOrWhiteSpace(d?.Frase) ? "—" : d!.Frase;

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("==============  💘 ¡MATCH!  ==============");
                Console.ResetColor();
                Console.WriteLine($"{"ID".PadRight(12)}: {u.Id}");
                Console.WriteLine($"{"Nombre".PadRight(12)}: {nombre}");
                Console.WriteLine($"{"Edad".PadRight(12)}: {edad}");
                Console.WriteLine($"{"Género".PadRight(12)}: {genero}");
                Console.WriteLine($"{"Profesión".PadRight(12)}: {profesion}");
                Console.WriteLine($"{"Intereses".PadRight(12)}: {intereses}");
                Console.WriteLine($"{"Frase".PadRight(12)}: {frase}");
                Console.WriteLine(new string('-', 42));
                Console.WriteLine("💬 Recomendación: envíale un mensaje amable para iniciar la conversación.");
                Console.WriteLine(new string('-', 42));
                Console.WriteLine("[N] Siguiente  [P] Anterior  [Q] Salir");
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.N:
                        idx = (idx + 1) % usuarios.Count;
                        break;
                    case ConsoleKey.P:
                        idx = (idx - 1 + usuarios.Count) % usuarios.Count;
                        break;
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


            var actor = await _context.Usuarios.SingleOrDefaultAsync(u => u.Id == usuarioId);
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


            if (esLike)
                await NotificarSiMatchAsync(usuarioId, usuarioLikedId);

            return true;
        }

        private async Task NotificarSiMatchAsync(int quienDaLikeId, int aQuienLeDiLikeId)
        {
            bool reciproco = await _context.Likes.AnyAsync(l =>
                l.UsuarioId == aQuienLeDiLikeId &&
                l.UsuarioLikedId == quienDaLikeId &&
                l.EsLike);

            if (!reciproco) return;

            var nombreOtro = await _context.Usuarios
                .Where(u => u.Id == aQuienLeDiLikeId)
                .Select(u => u.Nombre)
                .SingleAsync();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"💘 ¡Match! Tú y {nombreOtro} se dieron like.");
            Console.ResetColor();
            Console.WriteLine("💬 Recomendación: envíale un mensaje amable para iniciar la conversación.");
        }

        private Task<bool> DarLikeAsync(int usuarioId, int usuarioLikedId) =>
            ReaccionarAsync(usuarioId, usuarioLikedId, true);

        private Task<bool> DarDisLikeAsync(int usuarioId, int usuarioLikedId) =>
            ReaccionarAsync(usuarioId, usuarioLikedId, false);
            
         

public async Task VerEstadisticasSimpleAsync()
{
    Console.Clear();
    Console.WriteLine("=== 📈 Estadísticas ===");

    
    var topLikes = await _context.Likes
        .Where(l => l.EsLike)
        .GroupBy(l => l.UsuarioLikedId)
        .Select(g => new { UsuarioId = g.Key, Conteo = g.Count() })
        .OrderByDescending(x => x.Conteo)
        .FirstOrDefaultAsync();

    var topLikesNombre = topLikes == null
        ? null
        : await _context.Usuarios
            .Where(u => u.Id == topLikes.UsuarioId)
            .Select(u => u.Nombre)
            .FirstOrDefaultAsync();

    
    var topDislikes = await _context.Likes
        .Where(l => !l.EsLike)
        .GroupBy(l => l.UsuarioLikedId)
        .Select(g => new { UsuarioId = g.Key, Conteo = g.Count() })
        .OrderByDescending(x => x.Conteo)
        .FirstOrDefaultAsync();

    var topDislikesNombre = topDislikes == null
        ? null
        : await _context.Usuarios
            .Where(u => u.Id == topDislikes.UsuarioId)
            .Select(u => u.Nombre)
            .FirstOrDefaultAsync();

   
    var topMatches = await (
        from l1 in _context.Likes
        join l2 in _context.Likes
            on new { A = l1.UsuarioId, B = l1.UsuarioLikedId }
            equals new { A = l2.UsuarioLikedId, B = l2.UsuarioId }
        where l1.EsLike && l2.EsLike
        group l1 by l1.UsuarioId into g
        select new { UsuarioId = g.Key, Conteo = g.Select(x => x.UsuarioLikedId).Distinct().Count() }
    )
    .OrderByDescending(x => x.Conteo)
    .FirstOrDefaultAsync();

    var topMatchesNombre = topMatches == null
        ? null
        : await _context.Usuarios
            .Where(u => u.Id == topMatches.UsuarioId)
            .Select(u => u.Nombre)
            .FirstOrDefaultAsync();

    Console.WriteLine($"👍 Más likes   : {topLikesNombre   ?? "—"}");
    Console.WriteLine($"👎 Más dislikes: {topDislikesNombre ?? "—"}");
    Console.WriteLine($"💘 Más matches : {topMatchesNombre  ?? "—"}");

    Console.WriteLine("\nPresiona una tecla para continuar...");
    Console.ReadKey(true);
}

    }
}
