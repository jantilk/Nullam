using Application.Common;
using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface ISocialEventCompaniesService
{
    Task<OperationResult<List<GetCompaniesBySocialEventIdResponse>>?> GetCompaniesBySocialEventId(Guid eventId);
    Task<OperationResult<GetSocialEventCompanyResponse>?> GetByCompanyId(Guid socialEventId, Guid companyId);
    Task<OperationResult<bool>> Add(Guid socialEventId, AddSocialEventCompanyRequest request);
    Task<OperationResult<bool>> Update(Guid socialEventId, Guid companyId, UpdateSocialEventCompanyRequest request);
}