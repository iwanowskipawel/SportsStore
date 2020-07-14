using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using Xunit;

namespace SportStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Porducts()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[]
            {
                new Product{Name="P1", ProductID=1},
                new Product{Name="P2", ProductID=2},
                new Product{Name="P3", ProductID=3}
            }.AsQueryable<Product>());

            AdminController target = new AdminController(mock.Object);

            //act
            Product[] result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();

            //assert
            Assert.Equal(3, result.Count());
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);
        }

        public T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        [Fact]
        public void Can_Edit_Products()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{Name="P1", ProductID=1},
                new Product{Name="P2", ProductID=2},
                new Product{Name="P3", ProductID=3}
            }.AsQueryable<Product>());

            AdminController target = new AdminController(mock.Object);

            //act

            Product result = GetViewModel<Product>(target.Edit(2));

            //assert
            Assert.Equal(2, result.ProductID);
        }

        [Fact]
        public void Cannot_Edit_Nonexistance_Product()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{Name="P1", ProductID=1},
                new Product{Name="P2", ProductID=2},
                new Product{Name="P3", ProductID=3}
            }.AsQueryable<Product>());

            AdminController target = new AdminController(mock.Object);

            //act

            Product result = GetViewModel<Product>(target.Edit(4));

            //assert
            Assert.Null(result);
        }

        [Fact]
        public void Can_Save_Valid_Products()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };

            Product product = new Product { Name = "Test" };

            //act
            IActionResult result = target.Edit(product);

            //assert
            mock.Verify(m => m.SaveProduct(product));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Products()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };

            Product product = new Product { Name = "test" };

            target.ModelState.AddModelError("Error", "Error");

            //act

            IActionResult result = target.Edit(product);

            //assert
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Products()
        {
            //arrange
            Product product = new Product { ProductID = 1, Name = "p1" };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                product,
                new Product {ProductID =2, Name= "p2"},
                new Product {ProductID =3, Name= "p3"},
                new Product {ProductID =4, Name= "p4"},
            }.AsQueryable<Product>());

            AdminController target = new AdminController(mock.Object);

            //act
            target.Delete(product.ProductID);

            //assert
            mock.Verify(m => m.DeleteProduct(product.ProductID));
        }
    }
}
