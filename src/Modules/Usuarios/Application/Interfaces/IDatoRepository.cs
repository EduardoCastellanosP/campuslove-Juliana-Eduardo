using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Interfaces
{
    public interface IDatoRepository
    {
        Task<Dato?> GetByUsuarioIdAsync(int usuarioId);
        Task<bool> ExistsEmailAsync(string email);
        Task Add(Dato datos);
        Task SaveAsync();

        Task Update(Dato datos);
    }
}