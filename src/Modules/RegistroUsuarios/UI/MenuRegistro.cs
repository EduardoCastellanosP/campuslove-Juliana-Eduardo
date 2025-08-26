using System;
using System.Threading.Tasks;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Services;         // DatosService
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Infrastructure.Repositories;  // UsuarioRepository, DatoRepository
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.UI;
using campuslove_Juliana_Eduardo.src.Shared.Context;

namespace campuslove_Juliana_Eduardo.src.Modules.RegistroUsuarios.UI
{
    public class MenuRegistro
    {
        private readonly AppDbContext _context;
        private readonly UsuarioRepository _usuarioRepo;
         private readonly int _miUsuarioId;

        public MenuRegistro(AppDbContext context, int miUsuarioId)
        {
            _context = context;
            _usuarioRepo = new UsuarioRepository(_context);
            _miUsuarioId = miUsuarioId;
        }

        public async Task RenderMenu()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=== Menú de Registro de Usuarios ===");
                Console.WriteLine("1. Registrar Datos (perfil)");
                Console.WriteLine("2. Volver al Menú Principal");
                Console.Write("Seleccione una opción: ");
                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                       
                        var datoRepo = new DatoRepository(_context);
                        var datoService = new DatosService(datoRepo, _usuarioRepo);
                        var menuDatos = new MenuDatos(datoService, _context, _miUsuarioId);
                        await menuDatos.RenderMenuAsync();
                        break;

                    case "2":
                        salir = true;
                        break;

                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey(true);
                        break;
                }
            }
        }
    }
}
