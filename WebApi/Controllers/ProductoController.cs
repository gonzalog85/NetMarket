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
        public async Task<ActionResult<Pagination<ProductoDTO>>> GetProductos([FromQuery] ProductoSpecificationParams productoParams)
        {
            var spec = new ProductoWithCategoriaAndMarcaSpecification(productoParams);
            var productos = await productoRepository.GetAllWithSpec(spec);

            var specCount = new ProductoForCountingSpecification(productoParams);
            var totalProductos = await productoRepository.CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalProductos / productoParams.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoDTO>>(productos);

            var respuesta = new Pagination<ProductoDTO>
            {
                Count = totalProductos,
                PageIndex = productoParams.PageIndex,
                PageSize = productoParams.PageSize,
                PageCount = totalPages,
                Data = data
            };

            return Ok(respuesta);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDTO>> GetProducto(int id)
        {
            var spec = new ProductoWithCategoriaAndMarcaSpecification(id);
            var producto = await productoRepository.GetByIdWithSpec(spec);
            if (producto == null)
            {
                return NotFound(new CodeErrorResponse(404, "No se encontro el producto"));
            }
            return mapper.Map<Producto, ProductoDTO>(producto);
        }

    }
}
