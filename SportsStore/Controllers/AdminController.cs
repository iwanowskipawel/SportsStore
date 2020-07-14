using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index() => View(repository.Products);

        public ViewResult Edit(int productID)
            => View(repository.Products
                .FirstOrDefault(p => p.ProductID == productID));
        
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["Message"] = $"{product.Name} has been saved";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create() => View("Edit", new Product());

        [HttpPost]
        public IActionResult Delete(int productID)
        {
            Product deletedProduct = repository.DeleteProduct(productID);
            if (deletedProduct != null)
                TempData["Message"] = $"{deletedProduct.Name} was deleted";
            return RedirectToAction(nameof(Index));
        }

    }
}
