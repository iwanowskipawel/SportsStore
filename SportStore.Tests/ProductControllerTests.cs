using Microsoft.AspNetCore.Mvc;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]{
                new Product{ProductID=1, Name="p1"},
                new Product{ProductID=2, Name="p2"},
                new Product{ProductID=3, Name="p3"},
                new Product{ProductID=4, Name="p4"},
                new Product{ProductID=5, Name="p5"}
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            //Act
            ProductListViewModel result =
                controller.List(null, 2).ViewData.Model as ProductListViewModel;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("p4", prodArray[0].Name);
            Assert.Equal("p5", prodArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagingation_View_Model()
        {
            //Arrange
            Mock<IProductRepository> repository = new Mock<IProductRepository>();
            repository.Setup(x => x.Products)
                .Returns((new Product[]
                {
                    new Product{Name="P1", ProductID=1},
                    new Product{Name="P2", ProductID=2},
                    new Product{Name="P3", ProductID=3},
                    new Product{Name="P4", ProductID=4},
                    new Product{Name="P5", ProductID=5}
                }).AsQueryable<Product>());

            ProductController controller =
                new ProductController(repository.Object);

            //Act 
            ProductListViewModel result = controller.List(null, 2).Model as ProductListViewModel;

            //Asssert
            Assert.Equal(2, result.PagingInfo.CurrentPage);
            Assert.Equal(4, result.PagingInfo.ItemsPerPage);
            Assert.Equal(5, result.PagingInfo.TotalItems);
            Assert.Equal(2, result.PagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            //Arrange
            Mock<IProductRepository> repository = new Mock<IProductRepository>();
            repository.Setup(x => x.Products)
                .Returns((new Product[]
                {
                    new Product(){Name="p1", Category="k1"},
                    new Product(){Name="p2", Category="k1"},
                    new Product(){Name="p3", Category="k2"},
                    new Product(){Name="p4", Category="k2"},
                    new Product(){Name="p5", Category="k2"},
                    new Product(){Name="p6", Category="k2"},
                }).AsQueryable());

            ProductController controller = new ProductController(repository.Object);

            //Act
            ProductListViewModel result = controller.List("k1").Model as ProductListViewModel;

            //Assert
            Assert.Equal(2, result.Products.Count());
            Assert.Equal("k1", result.CurrentCategory);
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
            }).AsQueryable<Product>());

            ProductController target = new ProductController(mock.Object);

            Func<ViewResult, ProductListViewModel> GetModel = 
                result => result?.ViewData?.Model as ProductListViewModel;

            //Act
            int? val1 = GetModel(target.List("Cat1"))?.PagingInfo.TotalItems;
            int? val2 = GetModel(target.List("Cat2"))?.PagingInfo.TotalItems;
            int? val3 = GetModel(target.List("Cat3"))?.PagingInfo.TotalItems;
            int? val0 = GetModel(target.List(null))?.PagingInfo.TotalItems;

            //Assert
            Assert.Equal(2, val1);
            Assert.Equal(2, val2);
            Assert.Equal(1, val3);
            Assert.Equal(5, val0);
        }
    }
}
