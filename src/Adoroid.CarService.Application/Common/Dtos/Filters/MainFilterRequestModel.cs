using Adoroid.Core.Application.Requests;

namespace Adoroid.CarService.Application.Common.Dtos.Filters;

public record MainFilterRequestModel(PageRequest PageRequest, string? Search, DateTime? StartDate, DateTime? EndDate, Guid? CustomerId);
