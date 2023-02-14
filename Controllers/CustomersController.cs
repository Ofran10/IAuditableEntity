using Microsoft.AspNetCore.Mvc;
using FastDeliveryApi.Data;
using FastDeliveryApi.Entity;

namespace FastDeliveryApi.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly FastDeliveryDbContext _context;

    public CustomersController(FastDeliveryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Customer>> Get()
    {
        var customers = _context.Customers.ToList();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public ActionResult<IEnumerable<Customer>>  GetById(int id)
    {
        var customers = _context.Customers.Find(id);
        return Ok(customers);
    }

    [HttpPost]
    public ActionResult<IEnumerable<Customer>>  Post(Customer customers)
    {
        _context.Customers.Add(customers);
        _context.SaveChanges();
        return Created($"/id?id={customers.Id}", customers);
    }

    [HttpPut]
    public ActionResult<IEnumerable<Customer>>  Put(Customer customersToUpdate)
    {
        _context.Customers.Update(customersToUpdate);
        _context.SaveChanges();
         return Ok(customersToUpdate);
    }

    [HttpDelete("{id}")]
    public ActionResult<IEnumerable<Customer>>  Delete(int id)
    {
        var customersToDelete = _context.Customers.Find(id);
        if (customersToDelete == null)
        {
            return NotFound();
        }
        _context.Customers.Remove(customersToDelete);
        _context.SaveChanges();
        return Ok(customersToDelete);
    }
    
}
