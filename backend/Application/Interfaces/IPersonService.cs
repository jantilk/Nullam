using Application.DTOs;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface IPersonService
{
    Task<OperationResult<List<PersonResponse>>> Get(FilterDto? filter);
}