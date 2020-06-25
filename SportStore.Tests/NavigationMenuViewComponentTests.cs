using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Indicates_Selected_Category()
        {
            //Arrange
            string categoryToSelect = "c2";
            Mock<IProductRepository> repository = new Mock<IProductRepository>();
            repository.Setup(x => x.Products)
                .Returns((new Product[]{
                    new Product{ProductID = 1, Name="p1", Category = "c1"},
                    new Product{ProductID = 2, Name="p2", Category = "c2"},
                    new Product{ProductID = 3, Name="p3", Category = "c1"},
                    new Product{ProductID = 4, Name="p4", Category = "c2"},
                    new Product{ProductID = 5, Name="p5", Category = "c1"},
                    new Product{ProductID = 6, Name="p6", Category = "c3"},
                    new Product{ProductID = 7, Name="p7", Category = "c3"},
                    new Product{ProductID = 8, Name="p8", Category = "c2"},
                    }).AsQueryable<Product>);
            
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(repository.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                } 
            };
            target.RouteData.Values["category"] = categoryToSelect;
            //Act
            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            //Assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}
