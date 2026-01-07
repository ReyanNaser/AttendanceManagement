using Application.Common;
using Domain.DTOs;
using Infrastructure.UnitofWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Employee
{
    public class GetManager//: IEndpoint
    {
        //public void MapEndpoint(IEndpointRouteBuilder app)
        //{
        //    app.MapGet("/getmanager", Handler)
        //        .WithName("GetManager")
        //        .WithTags("Manager")
        //        .Produces<EmployeeResponse>(StatusCodes.Status200OK)
        //        .ProducesProblem(StatusCodes.Status404NotFound);
        //}
        //private static async Task<IResult> Handler(Guid Id, IUnitOfWork db, CancellationToken cancellationToken)
        //{
        //    var manager = await db.Manager.GetByIdAsync(Id);
        //    if (manager == null)
        //    {
        //        return Results.NotFound("Employee Not Found");
        //    }
        //    return Results.Ok(manager);
        //}
    }
}
