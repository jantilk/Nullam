using Domain.Entities;

namespace Application.Interfaces;

public interface ICompanyRepository
{
    Task Add(Company company);
    Task<Company?> Get(Guid companyId);
}