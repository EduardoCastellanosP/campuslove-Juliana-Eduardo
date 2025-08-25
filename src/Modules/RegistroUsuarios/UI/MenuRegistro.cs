using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Services;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Infrastructure.Repositories;
using campuslove_Juliana_Eduardo.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace campuslove_Juliana_Eduardo.src.Modules.RegistroUsuarios.UI
{
    public class MenuRegistro
    {
        private readonly AppDbContext _context;
        private readonly UsuarioService service;
        private readonly UsuarioRepository repo;
        public MenuRegistro(AppDbContext context)
        {
            _context = context;
            service = new UsuarioService(repo);
            repo = new UsuarioRepository(_context);
        }

        public async Task RenderMenu()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=== Menú de Registro de Usuarios ===");
                Console.WriteLine("1. Registrar Usuario");
                Console.WriteLine("2. Volver al Menú Principal");
                Console.Write("Seleccione una opción: ");
                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        await RegistrarUsuario();
                        break;
                    case "2":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
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



            Console.Write("¡Cual es su Género?: ");
            string genero = Console.ReadLine()!;

            Console.Write("¡Cual es su Profesion?: ");
            string profesion = Console.ReadLine()!;

            Console.Write("Ingrese sus intereses(Si son mas de uno, separelos con " , "): ");
            string intereses = Console.ReadLine()!;

            Console.Write("Frase de perfil: ");
            string frase = Console.ReadLine()!;

            Console.ForegroundColor = ConsoleColor.Yellow;
            await service.RegistrarUsuarioAsync(nombre, email, edad, genero, profesion, intereses, frase);
            Console.WriteLine($"\n✅ Usuario '{nombre}' registrado con éxito!");


            Console.ResetColor();
            Console.ReadKey();
        }



















    }
}