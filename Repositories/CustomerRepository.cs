using Microsoft.EntityFrameworkCore;

using FastDeliveryApi.Data;
using FastDeliveryApi.Entity;
using FastDeliveryApi.Repositories.Interfaces;

namespace FastDeliveryApi.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private FastDeliveryDbContext _dbContext;

    public CustomerRepository(FastDeliveryDbContext dbContext) => 
        _dbContext = dbContext;

    public void Add(Customer customer) =>
        _dbContext.Set<Customer>().Add(customer);

    public async Task<IReadOnlyCollection<Customer>> GetAll() =>
        await _dbContext
              .Set<Customer>()
              .ToListAsync();

    public async Task<Customer?> GetCustomerById(int Id, CancellationToken cancellationToken = default) =>
        await _dbContext
            .Set<Customer>()
            .FirstOrDefaultAsync(customer => customer.Id == Id, cancellationToken);

    public void Update(Customer customer) =>
        _dbContext.Set<Customer>().Update(customer);

    public void Delete(Customer customer) =>
        _dbContext.Set<Customer>().Remove(customer);
}