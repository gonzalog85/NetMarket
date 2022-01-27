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
    public class CarritoCompraController : BaseApiController
    {
        private readonly ICarritoCompraRepository carritoCompra;

        public CarritoCompraController(ICarritoCompraRepository carritoCompra)
        {
            this.carritoCompra = carritoCompra;
        }

        [HttpGet]
        public async Task<ActionResult<CarritoCompra>> GetCarritoCompraById(string id)
        {
            var carrito = await carritoCompra.GetCarritoCompraAsync(id);

            return Ok(carrito ?? new CarritoCompra(id));
        }

        [HttpPost]
        public async Task<ActionResult<CarritoCompra>> UpdateCarritoCompra(CarritoCompra carritoParametro)
        {
            var carritoActializado = await carritoCompra.UpdateCarritoCompraAsync(carritoParametro);

            return Ok(carritoActializado);
        }

        [HttpDelete]
        public async Task DeleteCarritoCompra(string id)
        {
            await carritoCompra.DeleteCarritoCompraAsync(id);
        }
    }
}
