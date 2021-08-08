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
using WebApi.Errors;

namespace WebApi.Controllers
{
    public class ProductoController : BaseApiController
    {
        private readonly IGenericRepository<Producto> productoRepository;
        private readonly IMapper mapper;

        public ProductoController(IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            this.productoRepository = productoRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductoDTO>>> GetProductos(string sort, int? marca, int? categoria)
        {
            var spec = new ProductoWithCategoriaAndMarcaSpecification(sort, marca, categoria);
            var productos = await productoRepository.GetAllWithSpec(spec);
            return Ok(mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoDTO>>(productos));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDTO>> GetProducto(int id)
        {
            var spec = new ProductoWithCategoriaAndMarcaSpecification(id);
            var producto = await productoRepository.GetByIdWithSpec(spec);
            if(producto == null)
            {
                return NotFound(new CodeErrorResponse(404, "No se encontro el producto"));
            }
            return mapper.Map<Producto, ProductoDTO>(producto);
        }

    }
}
