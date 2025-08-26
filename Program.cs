using System;
using System.Threading.Tasks;
using CampusLove.Modules.Usuarios.UI;
using campuslove_Juliana_Eduardo.src.Shared.Helpers;

var context = DbContextFactory.Create();


bool salir = false;

while (!salir)
{
   Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("+=======================================+");
    Console.WriteLine("|       Bienvenido a Campus Love ❤️      |");
    Console.WriteLine("+=======================================+");
    Console.WriteLine("|       1. Registro/Login               |");
    Console.WriteLine("|       2. Salir                        |");
    Console.WriteLine("+=======================================+");

    Console.Write("Seleccione una opción: ");
    string opcion = Console.ReadLine() ?? "";

    switch (opcion)
    {
        case "1":
            await new MenuUsuario(context).RenderMenu();
            
            break;

           
        case "2":
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
