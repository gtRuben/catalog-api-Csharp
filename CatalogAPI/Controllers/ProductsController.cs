﻿using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CatalogAPIContext _context;

        public ProductsController(CatalogAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var products = _context.Products.AsNoTracking().ToList();
            
            return (products == null) ? NotFound() : Ok(products);
        }

        [HttpGet("{id:int}", Name ="GetProduct")]
        public ActionResult Get(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            
            return (product == null) ? NotFound() : Ok(product);
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product == null) return BadRequest();

            _context.Products.Add(product);
            _context.SaveChanges();
            
            return new CreatedAtRouteResult("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            if (product.Id != id) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            
            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok();
        }
    }
}
