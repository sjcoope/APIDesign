using Microsoft.AspNetCore.Mvc;
using SJCNet.APIDesign.Data;
using SJCNet.APIDesign.Model;
using System;
using System.Linq;
using System.Net;

namespace SJCNet.APIDesign.API.Controllers
{
    // TODO: Update to use REpo pattern.

    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        public OrdersController()
        {}

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                using (var db = new DataContext())
                {
                    return Ok(db.Orders.ToList());
                }
            }
            catch(Exception ex)
            {
                // TODO: Implement logging
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }
        
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (var db = new DataContext())
            {
                var match = db.Products.SingleOrDefault(i => i.Id == id);

                if(match != null)
                {
                    return Ok(match);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            // TODO: Validate input

            using (var db = new DataContext())
            {
                var entityTracking = db.Products.Add(product);
                if(entityTracking.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    // TODO: properly format URI
                    return Created("", entityTracking.Entity);
                }

                return BadRequest();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
