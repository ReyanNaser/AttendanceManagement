using Microsoft.AspNetCore.Routing;

namespace Application.Common;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
