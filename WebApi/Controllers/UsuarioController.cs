using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Errors;

namespace WebApi.Controllers
{
    public class UsuarioController : BaseApiController
    {
        private readonly UserManager<Usuario> userManager;
        private readonly SignInManager<Usuario> signInManager;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDTO>> Login(LoginDTO loginDTO)
        {
            var usuario = await userManager.FindByEmailAsync(loginDTO.Email);

            if(User == null)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }

            var resultado = await signInManager.CheckPasswordSignInAsync(usuario, loginDTO.Password, false);

            if (!resultado.Succeeded)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }

            return new UsuarioDTO
            {
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = "Este es el token del usuario",
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido
            };
        }
    }
}
