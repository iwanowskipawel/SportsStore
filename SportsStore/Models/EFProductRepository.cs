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

        public Product DeleteProduct(int productID)
        {
            Product dbEntry = dbContext.Products
                .FirstOrDefault(p => p.ProductID == productID);
            if (dbEntry != null)
            {
                dbContext.Products.Remove(dbEntry);
                dbContext.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
                dbContext.Products.Add(product);
            else
            {
                Product dbEntry = dbContext.Products
                    .FirstOrDefault(p => p.ProductID == product.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
            }
            dbContext.SaveChanges();

        }
    }
}
