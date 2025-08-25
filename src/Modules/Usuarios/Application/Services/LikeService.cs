using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslove_Juliana_Eduardo.src.Modules.Usuarios.Domain.Entities;
using campuslove_Juliana_Eduardo.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace campuslove_Juliana_Eduardo.src.Modules.Usuarios.Application.Services;

public class LikeService
{
    private readonly AppDbContext _context;

    public LikeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task DarLikeAsync(int usuarioId, int usuarioLikedId)
    {
        bool existe = await _context.Likes
            .AnyAsync(l => l.UsuarioId == usuarioId && l.UsuarioLikedId == usuarioLikedId);

        if (existe)
            throw new Exception(" Ya diste like a este usuario.");

        var like = new Like
        {
            UsuarioId = usuarioId,
            UsuarioLikedId = usuarioLikedId
        };

        await _context.Likes.AddAsync(like);
        await _context.SaveChangesAsync();
    }

    public async Task<int> ContarLikesRecibidosAsync(int usuarioId)
{
    return await _context.Likes
        .CountAsync(l => l.UsuarioLikedId == usuarioId);
}

}
