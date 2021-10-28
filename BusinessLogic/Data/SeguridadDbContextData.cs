using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class SeguridadDbContextData
    {
        public static async Task SeedUserAsync(UserManager<Usuario> userManager)
        {
            if (!userManager.Users.Any())
            {
                var usuario = new Usuario
                {
                    Nombre = "Gonzalo",
                    Apellido = "Gomez",
                    UserName = "gonzalogomez22",
                    Email = "gonzalogomez22@gmail.com",
                    Direccion = new Direccion
                    {
                        Calle = "Saavedra Lamas 380",
                        Ciudad = "Yerba Buena",
                        CodigoPostal = "4107",
                        Departamento = "Yerba Buena"
                    }
                };
                await userManager.CreateAsync(usuario, "$Tommy1785");
            }
        }
    }
}
