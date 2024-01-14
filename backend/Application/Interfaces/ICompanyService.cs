using Application.DTOs;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface ICompanyService
{
    Task<OperationResult<List<CompanyResponse>>> Get(FilterDto? filter);
}