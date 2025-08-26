// src/Modules/Usuarios/Infrastructure/Repositories/DatosRepository.cs
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;
using campuslove_Juliana_Eduardo.src.Shared.Context;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces;

public class DatoRepository : IDatoRepository
{
    private readonly AppDbContext _context;
    public DatoRepository(AppDbContext context) => _context = context;

    // Lectura SIN tracking (para mostrar datos)
    public Task<Dato?> GetByUsuarioIdAsync(int usuarioId) =>
        _context.Datos.AsNoTracking().SingleOrDefaultAsync(d => d.UsuarioId == usuarioId);

    public Task<bool> ExistsEmailAsync(string email) =>
        _context.Datos.AnyAsync(d => d.Email == email);

    public async Task Add(Dato datos)
    {
        // Evita “otra instancia con la misma clave ya está siendo trackeada”
        var local = _context.Datos.Local.FirstOrDefault(d => d.UsuarioId == datos.UsuarioId);
        if (local != null)
            _context.Entry(local).State = EntityState.Detached;

        await _context.Datos.AddAsync(datos);
    }

    public Task SaveAsync() => _context.SaveChangesAsync();

    public Task Update(Dato datos)
    {
        // Si ya hay una instancia en el tracker, actualiza sus valores
        var local = _context.Datos.Local.FirstOrDefault(d => d.UsuarioId == datos.UsuarioId);
        if (local != null)
        {
            _context.Entry(local).CurrentValues.SetValues(datos);
        }
        else
        {
            // Si no hay instancia trackeada, adjunta y marca como modificada
            _context.Datos.Attach(datos);
            _context.Entry(datos).State = EntityState.Modified;
        }
        return Task.CompletedTask;
    }
}
