using Application.DTOs;
using Application.DTOs.Requests;
using Domain.Entities;

namespace Application.Interfaces;

public interface ICompanyRepository
{
    Task Add(Company company);
    Task<Company?> Get(GetCompanyRequest request);
    Task<List<Company>> Get(FilterDto? filter);
    Task<Company?> GetById(Guid companyId);
    Task<bool> Update(Company updatedCompany);
}