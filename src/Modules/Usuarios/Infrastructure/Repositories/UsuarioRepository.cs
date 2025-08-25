using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;
using campuslove_Juliana_Eduardo.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<IEnumerable<Usuario?>> GetAllAsync() =>
            await _context.Usuarios.ToListAsync();

        public async Task Add(Usuario entity) =>
            _context.Usuarios.Add(entity);

        public async Task Remove(Usuario entity)
        {
              _context.Usuarios.Remove(entity);

         }
          
        public async Task Update(Usuario entity)
         {
             _context.SaveChanges();
        }
            
        public async Task SaveAsync() =>
            await _context.SaveChangesAsync();
    }
}
