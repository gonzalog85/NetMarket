using BusinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly MarketDbContext context;

        public ProductoRepository(MarketDbContext context)
        {
            this.context = context;
        }

        public async Task<Producto> GetProductoByIdAsync(int id)
        {
            return await context.Producto.FindAsync(id);
        }

        public async Task<IReadOnlyList<Producto>> GetProductosAsync()
        {
            return await context.Producto.ToListAsync();
        }
    }
}
