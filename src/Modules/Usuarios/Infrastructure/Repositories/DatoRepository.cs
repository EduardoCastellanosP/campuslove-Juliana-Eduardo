// src/Modules/Usuarios/Infrastructure/Repositories/DatosRepository.cs
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;
using campuslove_Juliana_Eduardo.src.Shared.Context;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces;

public class DatoRepository : IDatoRepository
{
    private readonly AppDbContext _context;
    public DatoRepository(AppDbContext context) => _context = context;

    public Task<Dato?> GetByUsuarioIdAsync(int usuarioId) =>
        _context.Datos.AsNoTracking().SingleOrDefaultAsync(d => d.UsuarioId == usuarioId);

    public Task<bool> ExistsEmailAsync(string email) =>
        _context.Datos.AnyAsync(d => d.Email == email);

    public Task Add(Dato datos)
    {
        _context.Datos.Add(datos);
        return Task.CompletedTask;
    }

    public Task SaveAsync() => _context.SaveChangesAsync();

    public Task Update(Dato datos)
    {
        _context.Datos.Update(datos);
            return Task.CompletedTask;
    }
}
