using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class MarcaController : BaseApiController
    {
        private readonly IGenericRepository<Marca> marcaRepository;

        public MarcaController(IGenericRepository<Marca> marcaRepository)
        {
            this.marcaRepository = marcaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Marca>>> GetAllMarcas()
        {
            var marcas = await marcaRepository.GetAllAsync();
            return Ok(marcas);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Marca>> GetMarcaById(int id)
        {
            return await marcaRepository.GetByIdAsync(id);
        }
    }
}
