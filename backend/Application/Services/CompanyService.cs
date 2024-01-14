using Application.DTOs;
using Application.DTOs.Responses;
using Application.Interfaces;

namespace Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }
    
    public async Task<OperationResult<List<CompanyResponse>>> Get(FilterDto? filter)
    {
        try
        {
            var persons = await _companyRepository.Get(filter);
            
            var response = persons
                .Select(x => new CompanyResponse
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Name = x.Name,
                    RegisterCode = x.RegisterCode,
                    
                })
                .ToList();
            
            return OperationResult<List<CompanyResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            return OperationResult<List<CompanyResponse>>.FailureWithLog($"{nameof(Get)} operation failed. {ex.Message}");
        }
    }
}