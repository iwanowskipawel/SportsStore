using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_Products()
        {
            //Arrange
            Product p1 = new Product { ProductID = 1, Name = "p1" };
            Product p2 = new Product { ProductID = 2, Name = "p2" };

            Cart cart = new Cart();

            //Act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 2);
            cart.AddItem(p1, 1);
            cart.AddItem(p1, 3);

            CartLine[] result = cart.Lines.ToArray();
            //Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(5, result[0].Quantity);
            Assert.Equal(2, result[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Products()
        {
            //Arrange
            Product p1 = new Product { ProductID = 1, Name = "p1" };
            Product p2 = new Product { ProductID = 2, Name = "p2" };

            Cart cart = new Cart();

            //Act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 2);
            cart.AddItem(p1, 1);
            cart.AddItem(p1, 3);
            
            cart.RemoveLine(p1);

            CartLine[] result = cart.Lines.ToArray();

            //Assert
            Assert.Equal(1, result.Count());
            Assert.Equal("p2", result[0].Product.Name);
            Assert.Equal(0, result.Where(l => l.Product.Name == "p1").Count());
        }

        [Fact]
        public void Can_Compute_Total_Value()
        {
            //Arrange
            Product p1 = new Product { ProductID = 1, Name = "p1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "p2", Price = 10M };

            Cart cart = new Cart();

            //Act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 2);
            cart.AddItem(p1, 1);
            cart.AddItem(p1, 3);
            
            decimal result = cart.ComputeTotalValue();

            //Assert
            Assert.Equal(520M, result);
        }

        [Fact]
        public void Can_Clear_Cart()
        {
            //Arrange
            Product p1 = new Product { ProductID = 1, Name = "p1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "p2", Price = 10M };

            Cart cart = new Cart();

            //Act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 2);
            cart.AddItem(p1, 1);
            cart.AddItem(p1, 3);
            
            cart.Clear();

            //Assert
            Assert.Equal(0, cart.Lines.Count());
        }
    }
}
