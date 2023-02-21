using FastDeliveryApi.Entity;

namespace FastDeliveryApi.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<IReadOnlyCollection<Customer>> GetAll();
    Task<Customer?> GetCustomerById(int Id, CancellationToken cancellationToken = default);
    void Add(Customer customer);
    void Update(Customer customer);
    void Delete(Customer customer);
}