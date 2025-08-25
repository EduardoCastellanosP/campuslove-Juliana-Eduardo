using System;
using System.Threading.Tasks;
using CampusLove.Modules.Usuarios.UI;
using campuslove_Juliana_Eduardo.src.Shared.Helpers;

var context = DbContextFactory.Create();
var perfiles = new campuslove_Juliana_Eduardo.src.Modules.Perfiles.UI.Perfil(context);

bool salir = false;

while (!salir)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("=======================================");
    Console.WriteLine("        Bienvenido a Campus Love ❤️");
    Console.WriteLine("=======================================\n");
    Console.ResetColor();

    Console.WriteLine("1. Registro/Login");
    Console.WriteLine("2. Ver Perfiles");
    Console.WriteLine("3. Salir\n"); // <-- corregido (era 2)

    Console.Write("Seleccione una opción: ");
    string opcion = Console.ReadLine() ?? "";

    switch (opcion)
    {
        case "1":
            await new MenuUsuario(context).RenderMenu();
            
            break;

        case "2":
            if (!Sesion.UsuarioLogueado || Sesion.UsuarioId == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("No hay usuario logueado. Ingresa tu ID para ver perfiles (sólo para demo): ");
                if (!int.TryParse(Console.ReadLine(), out var idDemo))
                {
                    Console.WriteLine("ID inválido.");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
                }
                Sesion.UsuarioLogueado = true;
                Sesion.UsuarioId = idDemo;
            }

            await perfiles.VerPerfilesAsync(Sesion.UsuarioId.Value); // <-- pasa el Id
            break;

        case "3":
            salir = true; 
            Console.WriteLine("\nGracias por usar Campus Love. ¡Hasta luego!");
            break;

        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠ Opción no válida, inténtelo de nuevo.");
            Console.ResetColor();
            Console.ReadKey();
            break;
    }
}

public static class Sesion
{
    public static bool UsuarioLogueado { get; set; } = false;
    public static int? UsuarioId { get; set; } = null; // <-- guarda el Id
}
