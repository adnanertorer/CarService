using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Queries.GetSubTotals;


public record GetSubTotalQuery(Guid MainServiceId) : IRequest<Response<SubTotalModel>>;

public class GetSubTotalQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetSubTotalQuery, Response<SubTotalModel>>
{
    public async Task<Response<SubTotalModel>> Handle(GetSubTotalQuery request, CancellationToken cancellationToken)
    {
        var totalCost = await unitOfWork.SubServices.GetTotalCost(request.MainServiceId, cancellationToken);

        var subTotal = new SubTotalModel
        {
            TotalCost = totalCost.Item1 ?? 0m,
            TotalDiscount = totalCost.Item2 ?? 0m,
            TotalPrice = totalCost.Item3 ?? 0m
        };
        return Response<SubTotalModel>.Success(subTotal);
    }
}
