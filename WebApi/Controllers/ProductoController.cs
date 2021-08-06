using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IGenericRepository<Producto> productoRepository;
        private readonly IMapper mapper;

        public ProductoController(IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            this.productoRepository = productoRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductoDTO>>> GetProductos()
        {
            var spec = new ProductoWithCategoriaAndMarcaSpecification();
            var productos = await productoRepository.GetAllWithSpec(spec);
            return Ok(mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoDTO>>(productos));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDTO>> GetProducto(int id)
        {
            var spec = new ProductoWithCategoriaAndMarcaSpecification(id);
            var producto = await productoRepository.GetByIdWithSpec(spec);
            return mapper.Map<Producto, ProductoDTO>(producto);
        }

    }
}
