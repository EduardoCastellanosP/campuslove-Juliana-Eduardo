using System;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Services;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Infrastructure.Repositories;
using campuslove_Juliana_Eduardo.src.Shared.Context;
using Microsoft.EntityFrameworkCore;


namespace CampusLove.Modules.Usuarios.UI
{
    public class MenuUsuario
    {

        private readonly AppDbContext _context;
        readonly UsuarioRepository repo = null!;
        readonly UsuarioService service = null!;
        public MenuUsuario(AppDbContext context)
        {
            _context = context;
            repo = new UsuarioRepository(_context);
            service = new UsuarioService(repo);
        }



        public async Task RenderMenu()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=======================================");
                Console.WriteLine("        Módulo - Registro Usuarios");
                Console.WriteLine("=======================================\n");
                Console.ResetColor();

                Console.WriteLine("1. Registrar Usuario");
                Console.WriteLine("2. Modificar Usuario");
                Console.WriteLine("3. Eliminar Usuario");
                Console.WriteLine("4. Salir al Menú Principal\n");

                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        RegistrarUsuario();
                        break;
                    case "2":
                        ModificarUsuario();
                        break;
                    case "3":
                        EliminarUsuario();
                        break;
                    case "4":
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n Opción no válida, inténtelo de nuevo.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task RegistrarUsuario()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Registro de Usuario ===\n");
            Console.ResetColor();



            Console.Write("Nombre: ");
            string nombre = Console.ReadLine()!;

            int edad;
            while (true)
            {
                Console.Write("Ingresa la Edad: ");
                string inputEdad = Console.ReadLine()!;

                if (int.TryParse(inputEdad, out edad) && edad > 0 && edad < 100)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(" Por favor ingresa una edad válida.");
                }
            }

            Console.Write("Correo Electronico: ");
            string email = Console.ReadLine()!;

            var emailNorm = email.Trim().ToLowerInvariant();

            bool existe = await _context.Usuarios
                .AsNoTracking()
                .AnyAsync(u => u.Email.ToLower() == email);

            if (existe)
            {
                Console.WriteLine("Ese correo ya está registrado, intenta con otro.");

            }



            Console.Write("Género: ");
            string genero = Console.ReadLine()!;

            Console.Write("Profesion: ");
            string profesion = Console.ReadLine()!;

            Console.Write("Intereses: ");
            string intereses = Console.ReadLine()!;

            Console.Write("Frase de perfil: ");
            string frase = Console.ReadLine()!;

            Console.ForegroundColor = ConsoleColor.Yellow;
            await service.RegistrarUsuarioAsync(nombre, email, edad, genero, profesion, intereses, frase);
            Console.WriteLine($"\n✅ Usuario '{nombre}' registrado con éxito!");


            Console.ResetColor();
            Console.ReadKey();
        }






        private async Task ModificarUsuario()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Modificacion de Usuario ===\n");
            Console.ResetColor();


            Console.Write("Ingrese el ID del Usuario a actualizar: ");
            int idMod = int.Parse(Console.ReadLine()!);

            Console.Write("Ingrese el nuevo Nombre: ");
            string nuevoNombre = Console.ReadLine()!;

            int nuevaEdad;
            while (true)
            {
                Console.Write("Ingresa la Edad: ");
                string inputEdad = Console.ReadLine()!;

                if (int.TryParse(inputEdad, out nuevaEdad) && nuevaEdad > 0 && nuevaEdad < 100)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(" Por favor ingresa una edad válida.");
                }
            }

            Console.Write("Correo Electronico: ");
            string nuevoEmail = Console.ReadLine()!;



            Console.Write("Ingrese el nuevo género: ");
            string nuevoGenero = Console.ReadLine()!;

            Console.Write("Ingrese la nueva Profesion: ");
            string nuevaProfesion = Console.ReadLine()!;

            Console.Write("Intereses: ");
            string nuevoIntereses = Console.ReadLine()!;

            Console.Write("Frase de perfil: ");
            string nuevaFrase = Console.ReadLine()!;

            Console.ForegroundColor = ConsoleColor.Yellow;
            await service.ActualizarUsuarioAsync(idMod, nuevoNombre, nuevoEmail, nuevaEdad, nuevoGenero, nuevaProfesion, nuevoIntereses, nuevaFrase);
            Console.WriteLine($"\n✅ Usuario '{nuevoNombre}' actualizado con éxito!");


            Console.ResetColor();
            Console.ReadKey();
        }
    

         private async Task EliminarUsuario()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Eliminacion de Usuario ===\n");
            Console.ResetColor();

            
            Console.Write("Ingrese el ID del Usuario a eliminar: ");
            int idMod = int.Parse(Console.ReadLine()!);

            
            Console.ForegroundColor = ConsoleColor.Yellow;
            await service.EliminarUsuarioAsync(idMod);
            Console.WriteLine($"\n✅ Usuario '{idMod}' Eliminado con éxito!");
           

            Console.ResetColor();
            Console.ReadKey();
    }
 }
}