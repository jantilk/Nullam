using Application.DTOs;
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
    private readonly IResourceRepository _resourceRepository;

    public SocialEventCompaniesService(ISocialEventCompaniesRepository socialEventCompaniesRepository, ICompanyRepository companyRepository, ITransactionService transactionService, IResourceRepository resourceRepository)
    {
        _socialEventCompaniesRepository = socialEventCompaniesRepository;
        _companyRepository = companyRepository;
        _transactionService = transactionService;
        _resourceRepository = resourceRepository;
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
            await _transactionService.RollbackTransactionAsync();

            return OperationResult<bool>.Failure($"{nameof(Add)} operation failed. {ex}");
        }
    }
    
    public async Task<OperationResult<List<GetCompaniesBySocialEventIdResponse>>> GetBySocialEventId(Guid socialEventId)
    {
        try
        {
            var socialEventCompanies = await _socialEventCompaniesRepository.GetCompaniesBySocialEventId(socialEventId);
            
            var response = socialEventCompanies
                .Select(x => new GetCompaniesBySocialEventIdResponse
                {
                    Id = x.CompanyId,
                    CreatedAt = x.CreatedAt,
                    Name = x.Company.Name,
                    RegisterCode = x.Company.RegisterCode
                })
                .ToList();
            
            return OperationResult<List<GetCompaniesBySocialEventIdResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            return OperationResult<List<GetCompaniesBySocialEventIdResponse>>.FailureWithLog($"{nameof(GetBySocialEventId)} operation failed. {ex.Message}");
        }
    }
    
    public async Task<OperationResult<GetSocialEventCompanyResponse>?> GetByCompanyId(Guid socialEventId, Guid companyId)
    {
        try
        {
            var socialEventCompany = await _socialEventCompaniesRepository.GetByCompanyId(socialEventId, companyId);
            if (socialEventCompany == null)
            {
                return null;
            }

            var response = new GetSocialEventCompanyResponse
            {
                SocialEventId = socialEventCompany.SocialEventId,
                CompanyId = socialEventCompany.CompanyId,
                NumberOfParticipants = socialEventCompany.NumberOfParticipants,
                CreatedAt = socialEventCompany.CreatedAt,
                PaymentType = socialEventCompany.PaymentType,
                AdditionalInfo = socialEventCompany.AdditionalInfo,
                Company = new CompanyResponse
                {
                    Id = socialEventCompany.Company.Id,
                    CreatedAt = socialEventCompany.Company.CreatedAt,
                    Name = socialEventCompany.Company.Name,
                    RegisterCode = socialEventCompany.Company.RegisterCode
                },
            };
            
            return OperationResult<GetSocialEventCompanyResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return OperationResult<GetSocialEventCompanyResponse>.Failure($"{nameof(GetByCompanyId)} operation failed. {ex}");
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

            var paymentType = await _resourceRepository.GetById(request.PaymentTypeId);
            if (paymentType == null) {
                return OperationResult<bool>.Failure("PaymentType not found");
            }
            
            company.Name = request.Name;
            company.RegisterCode = request.RegisterCode;
            
            await _companyRepository.Update(company);

            socialEventCompany.PaymentType = paymentType;
            socialEventCompany.NumberOfParticipants = request.NumberOfParticipants;
            socialEventCompany.AdditionalInfo = request.AdditionalInfo;
            
            await _socialEventCompaniesRepository.Update(socialEventCompany);

            await _transactionService.CommitTransactionAsync();
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await _transactionService.RollbackTransactionAsync();
            
            return OperationResult<bool>.Failure($"{nameof(Update)} operation failed. {ex}");
        }
    }

    public async Task<OperationResult<bool>> Delete(Guid socialEventId, Guid companyId)
    {
        try
        {
            var socialEventCompany = await _socialEventCompaniesRepository.GetByCompanyId(socialEventId, companyId);

            if (socialEventCompany == null)
            {
                return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed. Social event company not found");
            }
            
            if (socialEventCompany.SocialEvent.Date < DateTime.UtcNow) {
                return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed. Cannot delete company from past event.");
            }
            
            var result = await _socialEventCompaniesRepository.Delete(socialEventCompany);

            if (!result) {
                return OperationResult<bool>.Failure($"{nameof(Delete)} operation failed.");
            }
            
            return OperationResult<bool>.Success(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}