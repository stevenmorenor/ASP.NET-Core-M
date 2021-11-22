using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class DataPrueba
    {
        public static async Task InsertarData(CursosOnlineContext context, UserManager<Usuario> usuarioManager)
        {
            if(!usuarioManager.Users.Any())
            {
                var usuario = new Usuario{NombreCompleto = "Vaxi Drez", UserName = "vazidrez", Email="vaxi.drez@gmail.com"};
                await usuarioManager.CreateAsync(usuario, "Password123$");
            }
        }
    }
}