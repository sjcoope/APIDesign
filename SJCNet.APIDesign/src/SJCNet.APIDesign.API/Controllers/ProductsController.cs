﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SJCNet.APIDesign.API.Utility;
using SJCNet.APIDesign.Data;
using SJCNet.APIDesign.Data.Repository;
using SJCNet.APIDesign.Model;
using SJCNet.APIDesign.Model.Validation;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SJCNet.APIDesign.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private IRepository<Product> _repository;

        public ProductsController(IRepository<Product> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get(string sort = "name", int page = 0, int pageSize = 0)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var products = _repository.Get();

                    // Add sorting
                    var productsResult = products.ApplySort(sort);

                    // Add pagination
                    var paginationHelper = new PaginationHelper(products.Count(), pageSize, page);
                    if (paginationHelper.PaginationInUse)
                    {
                        HttpContext.Response.Headers.Add("X-Pagination", paginationHelper.GetInfo());

                        productsResult = productsResult
                            .Skip(paginationHelper.SkipCount)
                            .Take(paginationHelper.PageSize);
                    }
                    
                    return Ok(productsResult.ToList());
                }
            }
            catch (Exception ex)
            {
                // TODO: Logging would be implemented here.
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                if(id == 0)
                {
                    return BadRequest();
                }

                using (var db = new DataContext())
                {
                    var match = _repository.Get(id);

                    if (match != null)
                    {
                        return Ok(match);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Logging would be implemented here.
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            try
            {
                var validator = new ProductValidator();
                var results = validator.Validate(product);

                if(results.IsValid)
                {
                    if (_repository.Add(product))
                    {
                        // TODO: properly format URI
                        var entityUri = new Uri($"{HttpContext.Request.Path}/{product.Id}");
                        return Created(entityUri, product);
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                // TODO: Logging would be implemented here.
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Product product)
        {
            try
            {
                var validator = new ProductValidator();
                var results = validator.Validate(product);

                if (results.IsValid)
                {
                    if (_repository.Get(id) == null)
                    {
                        return NotFound();
                    }

                    if (_repository.Update(product))
                    {
                        return Ok(product);
                    }
                }

                return BadRequest();
               
            }
            catch (Exception ex)
            {
                // TODO: Logging would be implemented here.
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_repository.Get(id) == null)
                {
                    return NotFound();
                }

                if (_repository.Delete(id))
                {
                    return NoContent();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                // TODO: Logging would be implemented here.
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody]JsonPatchDocument productPatchDocument)
        {
            try
            {
                // Validate parameter
                if (productPatchDocument != null)
                {
                    // TODO: Implement validation on patch.
                    var product = _repository.Get(id);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    
                    productPatchDocument.ApplyTo(product);
                    if (_repository.Update(product))
                    {
                        return Ok(product);
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                // TODO: Logging would be implemented here.
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}