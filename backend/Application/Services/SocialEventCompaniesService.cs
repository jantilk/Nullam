using Application.Common;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class SocialEventCompaniesService : ISocialEventCompaniesService
{
    private readonly ISocialEventCompaniesRepository _socialEventCompaniesRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly ITransactionService _transactionService;

    public SocialEventCompaniesService(ISocialEventCompaniesRepository socialEventCompaniesRepository, ICompanyRepository companyRepository, ITransactionService transactionService)
    {
        _socialEventCompaniesRepository = socialEventCompaniesRepository;
        _companyRepository = companyRepository;
        _transactionService = transactionService;
    }
    
    public async Task<OperationResult<List<GetCompaniesBySocialEventIdResponse>>?> GetCompaniesBySocialEventId(Guid socialEventId)
    {
        try
        {
            var socialEventCompanies = await _socialEventCompaniesRepository.GetCompaniesBySocialEventId(socialEventId);
            
            if (socialEventCompanies.Count == 0)
            {
                return null;
            }

            var response = socialEventCompanies
                .Select(x => new GetCompaniesBySocialEventIdResponse
                {
                    Id = x.CompanyId,
                    Name = x.Company.Name,
                    RegisterCode = x.Company.RegisterCode
                })
                .ToList();
            
            return OperationResult<List<GetCompaniesBySocialEventIdResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            return OperationResult<List<GetCompaniesBySocialEventIdResponse>>.FailureWithLog($"Failed to get social event companies! {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> Add(Guid socialEventId, AddSocialEventCompanyRequest request)
    {
        try
        {
            await _transactionService.BeginTransactionAsync();

            var companyId = Guid.NewGuid();
            var company = new Company
            {
                Id = companyId,
                CreatedAt = DateTime.Now,
                Name = request.Name,
                RegisterCode = request.RegisterCode
            };
            
            await _companyRepository.Add(company);
            await _socialEventCompaniesRepository.Add(socialEventId, companyId, request);
            
            await _transactionService.CommitTransactionAsync();
            return OperationResult<bool>.Success(true); 
        }
        catch (Exception ex)
        {
            // TODO: how to check here if current transaction is null
            await _transactionService.RollbackTransactionAsync();

            return OperationResult<bool>.Failure($"Failed to add social event company! {ex}");
        }
    }

    public async Task<OperationResult<bool>> Update(Guid socialEventId, Guid companyId, UpdateSocialEventCompanyRequest request)
    {
        try
        {
            await _transactionService.BeginTransactionAsync();

            var socialEventCompany = await _socialEventCompaniesRepository.GetSocialEventCompany(socialEventId, companyId);
            if (socialEventCompany == null) {
              return OperationResult<bool>.Failure("Social event company not found");
            }
            
            var company = await _companyRepository.Get(companyId);
            if (company == null) {
                return OperationResult<bool>.Failure("Company not found");
            }

            var updatedSocialEventCompany = new SocialEventCompany
            {
                SocialEventId = socialEventCompany.SocialEventId,
                CompanyId = socialEventCompany.CompanyId,
                CreatedAt = socialEventCompany.CreatedAt,
                PaymentType = request.PaymentType,
                AdditionalInfo = request.AdditionalInfo,
                NumberOfParticipants = request.NumberOfParticipants,
            };
            
            await _socialEventCompaniesRepository.Update(updatedSocialEventCompany);

            await _transactionService.CommitTransactionAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await _transactionService.RollbackTransactionAsync();
            
            return OperationResult<bool>.Failure($"Failed to update social event company! {ex}");
        }
    }
}