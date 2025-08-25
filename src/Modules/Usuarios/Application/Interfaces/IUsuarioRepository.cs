using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByIdAsync(int id);
        Task<IEnumerable<Usuario?>> GetAllAsync();
        Task Add(Usuario entity);
        Task Update(Usuario entity);
        Task Remove(Usuario entity);
        Task SaveAsync();
    }
}