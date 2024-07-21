using AutoMapper;
using Mango.Services.ProductAPI.Controllers;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.API.Tests.ProductAPITest
{
    [TestClass]
    public class ProductAPIControllerTest
    {
        private AppDbContext _context;
        private Mock<IMapper> _mockMapper;
        private ProductAPIController _controller;
        private ResponseDTO _responseDTO;


        [TestInitialize]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _responseDTO = new ResponseDTO();
            _controller = new ProductAPIController(_context, _mockMapper.Object);
        }

        [TestMethod]
        public void Get_ReturnsMappedProductList_WhenNoException()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductId = 1,
                Name = "Samosa",
                Price = 15,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://placehold.co/603x403",
                CategoryName = "Appetizer" },

                new Product { ProductId = 2,
                Name = "Paneer Tikka",
                Price = 13.99,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://placehold.co/602x402",
                CategoryName = "Appetizer" }
            };

            _context.Products.AddRange(products);
            _context.SaveChanges();

            var productDTOs = new List<ProductDTO>
            {
                new ProductDTO { ProductId = 1,
                Name = "Samosa",
                Price = 15,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://placehold.co/603x403",
                CategoryName = "Appetizer" },
                new ProductDTO { ProductId = 2,
                Name = "Paneer Tikka",
                Price = 13.99,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://placehold.co/602x402",
                CategoryName = "Appetizer" }
            };

            _mockMapper.Setup(mapper => mapper.Map<List<ProductDTO>>(It.IsAny<List<Product>>())).Returns(productDTOs);

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(productDTOs, result.Result);
        }

        [TestMethod]
        public void Get_ReturnsErrorResponse_WhenExceptionThrown()
        {
            // Arrange
            var exceptionMessage = "An error occurred";
            _mockMapper.Setup(mapper => mapper.Map<List<ProductDTO>>(It.IsAny<List<Product>>())).Throws(new Exception(exceptionMessage));

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(exceptionMessage, result.Message);
        }
    }
}
