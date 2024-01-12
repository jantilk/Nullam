using Application.DTOs.Requests;
using Domain.Entities;

namespace Application.Interfaces;

public interface ISocialEventCompaniesRepository
{
    Task Add(Guid socialEventId, Guid companyId, AddSocialEventCompanyRequest request);
    Task<List<SocialEventCompany>> GetCompaniesBySocialEventId(Guid socialEventId);
    Task<SocialEventCompany?> GetByCompanyId(Guid socialEventId, Guid companyId);
    Task<SocialEventCompany?> GetSocialEventCompany(Guid socialEventId, Guid companyId);
    Task<bool> Update(SocialEventCompany updatedSocialEventCompany);
    Task<bool> Delete(SocialEventCompany socialEventCompany);
    Task<List<SocialEventCompany>> GetByResourceId(Guid resourceId);
}