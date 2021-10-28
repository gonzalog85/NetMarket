using Core.Entities;
using Core.Interfaces;
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
        private readonly ITokenService tokenService;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
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
                Token = tokenService.CreateToken(usuario),
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido
            };
        }
    
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDTO>> Registrar(RegistrarDTO registrarDTO)
        {
            var usuario = new Usuario
            {
                Email = registrarDTO.Email,
                UserName = registrarDTO.Username,
                Nombre = registrarDTO.Nombre,
                Apellido = registrarDTO.Apellido
            };

            var resultado = await userManager.CreateAsync(usuario, registrarDTO.Password);

            if (!resultado.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400));
            }

            return new UsuarioDTO
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Token = tokenService.CreateToken(usuario),
                Email = usuario.Email,
                Username = usuario.UserName
            };
        }
    }
}
