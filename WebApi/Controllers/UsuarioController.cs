using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Errors;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    public class UsuarioController : BaseApiController
    {
        private readonly UserManager<Usuario> userManager;
        private readonly SignInManager<Usuario> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ITokenService tokenService, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario()
        {
            //var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            //var usuario = await userManager.FindByEmailAsync(email);

            var usuario = await userManager.BuscarUsuarioAsync(HttpContext.User);

            return new UsuarioDTO
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = tokenService.CreateToken(usuario)
            };
        }

        [HttpGet("emailvalido")]
        public async Task<ActionResult<bool>> ValidarEmail ([FromQuery]string email)
        {
            var usuario = await userManager.FindByEmailAsync(email);

            if (usuario == null) return false;

            return true;
        }

        [Authorize]
        [HttpGet("direccion")]
        public async Task<ActionResult<DireccionDTO>> GetDireccion()
        {
            //var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            //var usuario = await userManager.FindByEmailAsync(email);

            var usuario = await userManager.BuscarUsuarioConDireccionAsync(HttpContext.User);

            return mapper.Map<Direccion, DireccionDTO>(usuario.Direccion);
        }

        [Authorize]
        [HttpPut("direccion")]
        public async Task<ActionResult<DireccionDTO>> UpdateDireccion(DireccionDTO direccion)
        {
            var usuario = await userManager.BuscarUsuarioConDireccionAsync(HttpContext.User);

            usuario.Direccion = mapper.Map<DireccionDTO, Direccion>(direccion);
            
            var resultado = await userManager.UpdateAsync(usuario);
            if (resultado.Succeeded) return Ok(mapper.Map<Direccion, DireccionDTO>(usuario.Direccion));

            return BadRequest("No se pudo actualizar la direccion del usuario");
        }
    }
}
