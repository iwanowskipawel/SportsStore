using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFProductRepository : IProductRepository
    {
        ApplicationDbContext dbContext;
        public EFProductRepository(ApplicationDbContext context)
        {
            this.dbContext = context;
        }
        IQueryable<Product> IProductRepository.Products => dbContext.Products;
    }
}
