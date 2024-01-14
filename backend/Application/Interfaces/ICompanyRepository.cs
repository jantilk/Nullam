using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface ICompanyRepository
{
    Task Add(Company company);
    Task<List<Company>> Get(FilterDto? filter);
    Task<Company?> GetById(Guid companyId);
    Task<Company?> GetByRegisterCode(int registerCode);
    Task<bool> Update(Company updatedCompany);
}