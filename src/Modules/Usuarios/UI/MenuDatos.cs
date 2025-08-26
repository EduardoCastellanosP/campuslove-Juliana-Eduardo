// src/Modules/Usuarios/UI/MenuDatos.cs
using System;
using System.Threading.Tasks;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.UI
{
    public class MenuDatos
    {
        private readonly IDatoService _datoService;
        private readonly int _usuarioId; // <- llega desde el login / menú principal

        public MenuDatos(IDatoService datoService, int usuarioId)
        {
            _datoService = datoService;
            _usuarioId = usuarioId;
        }

        public async Task RenderMenuAsync()
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=== Menú Datos ===");
                Console.WriteLine("1. Registrar datos (perfil)");
                Console.WriteLine("2. Actualizar datos (perfil)");
                Console.WriteLine("3. Volver");
                Console.Write("Opción: ");
                var op = Console.ReadLine();

                switch (op)
                {
                    case "1":
                        await RegistrarDatosAsync_Menu();
                        break;
                    case "2":
                        await ActualizarDatosAsync_Menu();
                        break;
                    case "3":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción inválida. Presiona una tecla...");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        // ---------- Helpers mínimos ----------
        private static string PedirObligatorio(string label)
        {
            while (true)
            {
                Console.Write($"{label}: ");
                var v = (Console.ReadLine() ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(v)) return v;
                Console.WriteLine("⚠️ Campo obligatorio.");
            }
        }

        private static int PedirEnteroPositivo(string label, int max = 120)
        {
            while (true)
            {
                Console.Write($"{label}: ");
                var t = Console.ReadLine();
                if (int.TryParse(t, out var n) && n > 0 && n < max) return n;
                Console.WriteLine($"⚠️ Ingresa un número válido (1-{max - 1}).");
            }
        }

        private static string PedirEmail(string label)
        {
            while (true)
            {
                Console.Write($"{label}: ");
                var e = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
                if (!string.IsNullOrWhiteSpace(e) && e.Contains("@") && e.Contains(".")) return e;
                Console.WriteLine("⚠️ Email inválido.");
            }
        }

        // ---------- Registrar (NO pide ID) ----------
        private async Task RegistrarDatosAsync_Menu()
        {
            Console.Clear();
            Console.WriteLine("=== Registrar datos (perfil) ===");

            var nombre     = PedirObligatorio("Nombre");
            var email      = PedirEmail("Email");
            var edad       = PedirEnteroPositivo("Edad");
            var genero     = PedirObligatorio("Género");
            var profesion  = PedirObligatorio("Profesión");
            Console.Write("Intereses (separados por coma): ");
            var intereses  = (Console.ReadLine() ?? "").Trim();
            var frase      = PedirObligatorio("Frase");

            try
            {
                await _datoService.RegistrarDatosAsync(
                    _usuarioId, nombre, email, edad, genero, profesion, intereses, frase);

                Console.WriteLine("✅ Datos registrados.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {ex.Message}");
            }
            Console.WriteLine("Presiona una tecla para continuar...");
            Console.ReadKey(true);
        }

        // ---------- Actualizar (NO pide ID) ----------
        private async Task ActualizarDatosAsync_Menu()
        {
            Console.Clear();
            Console.WriteLine("=== Actualizar datos (perfil) ===");

            var nuevoNombre   = PedirObligatorio("Nuevo nombre");
            var nuevoEmail    = PedirEmail("Nuevo email");
            var nuevaEdad     = PedirEnteroPositivo("Nueva edad");
            var nuevoGenero   = PedirObligatorio("Nuevo género");
            var nuevaProf     = PedirObligatorio("Nueva profesión");
            Console.Write("Nuevos intereses (separados por coma): ");
            var nuevosInter   = (Console.ReadLine() ?? "").Trim();
            var nuevaFrase    = PedirObligatorio("Nueva frase");

            try
            {
                await _datoService.ActualizarDatosAsync(
                    _usuarioId, nuevoNombre, nuevoEmail, nuevaEdad, nuevoGenero, nuevaProf, nuevosInter, nuevaFrase);

                Console.WriteLine("✅ Datos actualizados.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {ex.Message}");
            }
            Console.WriteLine("Presiona una tecla para continuar...");
            Console.ReadKey(true);
        }
    }
}
