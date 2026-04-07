using Application.Common;
using Domain.DTOs;
using Domain.Entities;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Employee
{
    public class AddManager//: IEndpoint
    {
        //public void MapEndpoint(IEndpointRouteBuilder app)
        //{
        //    app.MapPost("/addmanager", Handler)
        //        .WithName("CreateManager")
        //        .WithTags("Manager")
        //        .Produces<EmployeeResponse>(StatusCodes.Status201Created)
        //        .ProducesProblem(StatusCodes.Status400BadRequest);
        //}
        //private static async Task<IResult> Handler(AddManagerDto request,IAttendanceDbContext db,CancellationToken cancellationToken)
        //{
        //    var employeeexists = await db.Employees
        //        .AnyAsync(e => e.Id == request.EmployeeId, cancellationToken);

        //    if (!employeeexists)
        //    {
        //        return Results.BadRequest("Employee does not exist");
        //    }

        //    var managerexists = await db.Manager
        //        .AnyAsync(e => e.EmployeeId == request.EmployeeId, cancellationToken);

        //    if (managerexists)
        //    {
        //        return Results.BadRequest("Manager already exist");
        //    }

        //    var manager = new Manager
        //    {
        //        EmployeeId=request.EmployeeId,
        //        FirstName=request.FirstName,
        //        LastName=request.LastName,
        //        Designation=request.Designation
        //    };

        //    await db.Manager.AddAsync(manager);
        //    await db.SaveChangesAsync(cancellationToken);

        //    return Results.Created("Manager created successfully", manager);
        //}
    }
}
