﻿using CommerceAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;
using CommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommerceAPI.Controllers
{
    [Route("/api")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CommerceApiContext _context;

        public ProductsController(CommerceApiContext context)
        {
            _context = context;
        }

        [HttpGet("products/{productId}")]
        public ActionResult<Product> GetProduct_ReturnsSingleProduct(int productId)
        {
            return _context.Products.Where(e => e.Id == productId).Include(e => e.Merchant).Single();
        }

        [HttpPost("merchants/{merchantId}/products")]
        public ActionResult<Product> PostProduct_CreatesProductInDb(Product product, int merchantId)
        {
            _context.Merchants.Include(e => e.Products).Where(e => e.Id == merchantId).Single().Products.Add(product);
            _context.SaveChanges();

            return product;
        }

        [HttpPut("merchants/{merchantId}/products/{productId}")]
        public ActionResult<Product> PutProduct_UpdatesProductInDb(int productId, Product product)
        {
            var oldProduct = _context.Products.Find(productId);

            oldProduct.Name = product.Name;
            oldProduct.Description = product.Description;
            oldProduct.Category = product.Category;
            oldProduct.PriceInCents = product.PriceInCents;
            oldProduct.StockQuantity = product.StockQuantity;
            oldProduct.Merchant = product.Merchant;
            _context.Products.Update(oldProduct);

            _context.SaveChanges();

            return oldProduct;
        }

        [HttpDelete("products/{productId}")]
        public string DeleteProduct_RemovesProductFromDb_AndMerchant(int productId)
        {
            _context.Products.Remove(_context.Products.Find(productId));

            return "done.";
        }

        //[HttpGet("products")]
        //public ActionResult<IEnumerable<Product>> GetProducts()
        //{
        //    return _context.Products;
        //}


    }
}