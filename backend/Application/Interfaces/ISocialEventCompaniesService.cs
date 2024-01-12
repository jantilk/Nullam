using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface ISocialEventCompaniesService
{
    Task<OperationResult<bool>> Add(Guid socialEventId, AddSocialEventCompanyRequest request);
    Task<OperationResult<List<GetCompaniesBySocialEventIdResponse>>> GetBySocialEventId(Guid eventId);
    Task<OperationResult<GetSocialEventCompanyResponse>?> GetByCompanyId(Guid socialEventId, Guid companyId);
    Task<OperationResult<bool>> Update(Guid socialEventId, Guid companyId, UpdateSocialEventCompanyRequest request);
    Task<OperationResult<bool>> Delete(Guid socialEventId, Guid companyId);
}