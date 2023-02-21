using Microsoft.AspNetCore.Mvc;
using Mapster;

using FastDeliveryApi.Data;
using FastDeliveryApi.Entity;
using FastDeliveryApi.Repositories.Interfaces;
using FastDeliveryApi.Models;
using FastDeliveryApi.Exeption;

namespace FastDeliveryApi.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomersController(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> Get()
    {
        var customers = await _customerRepository.GetAll();
        return Ok(customers);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = request.Adapt<Customer>();
        
        // var customer = new Customer(request.Name,
        //     request.PhoneNumber,
        //     request.Email,
        //     request.Address
        // );
        
        _customerRepository.Add(customer);

        await _unitOfWork.SaveChangesAsync();

        var response = customer.Adapt<CustomerResponse>();

        return CreatedAtAction(
            nameof(GetCustomerById),
            new { id = response.Id },
            response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        if(request.Id != id)
        {
            throw new BadRequestException("Body Id is not equal than Url Id");
        }

        var customer = await _customerRepository.GetCustomerById(id);
        if(customer is null)
        {
            throw new NotFoundException("Customer", id);
        }

        customer.ChangeName(request.Name);
        customer.ChangePhoneNumber(request.PhoneNumber);
        customer.ChangeEmail(request.Email);
        customer.ChangeAddress(request.Address);
        customer.ChangeStatus(request.Status);
        
        _customerRepository.Update(customer);

        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult>  GetCustomerById(int id, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetCustomerById(id, cancellationToken);
        if(customer is null)
        {
            throw new NotFoundException("Customer", id);
        }

        var response = customer.Adapt<CustomerResponse>();

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomer(int id, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetCustomerById(id);
        if(customer is null)
        {
            return NotFound($"Customer Not Found With the Id {id}");
        }
        
        _customerRepository.Delete(customer);

        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // [HttpDelete("{id}")]
    // public ActionResult<IEnumerable<Customer>>  Delete(int id)
    // {
    //     var customersToDelete = _context.Customers.Find(id);
    //     if (customersToDelete == null)
    //     {
    //         return NotFound();
    //     }
    //     _context.Customers.Remove(customersToDelete);
    //     _context.SaveChanges();
    //     return NoContent();
    // }
    
}
