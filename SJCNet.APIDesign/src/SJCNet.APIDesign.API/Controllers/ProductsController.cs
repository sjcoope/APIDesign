using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SJCNet.APIDesign.API.Utility;
using SJCNet.APIDesign.Data;
using SJCNet.APIDesign.Data.Repository;
using SJCNet.APIDesign.Model;
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

        // TODO: Do we need to use async controller actions?

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

        /// <summary>
        /// Get a product by an id
        /// </summary>
        /// <param name="id">Id of the product to return</param>
        /// <remarks>Find a product</remarks>
        /// <response code="200">Ok</response>
        /// <response code="404">Resource not found</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
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
                // TODO: Validate input
                // IF NOT VALID return BadRequest()

                if (_repository.Add(product))
                {
                    // TODO: properly format URI
                    var entityUri = new Uri($"{HttpContext.Request.Path}/{product.Id}");
                    return Created(entityUri, product);
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
                // TODO: Validate input
                // IF NOT VALID return BadRequest()

                if (_repository.Get(id) == null)
                {
                    return NotFound();
                }

                if (_repository.Update(product))
                {
                    return Ok(product);
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

        /*
        [
            {
              "op": "replace",
              "path": "/Name",
              "value": "Test123"
            }
        ]
        */

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody]JsonPatchDocument productPatchDocument)
        {
            try
            {
                // Validate parameter
                if (productPatchDocument != null)
                {

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