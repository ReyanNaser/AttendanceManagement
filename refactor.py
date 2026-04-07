import os
import re

features_dir = r"c:\EmployeeAttendanceManagement\AttendanceManagement\Application\Features"

for root, _, files in os.walk(features_dir):
    for f in files:
        if f.endswith(".cs") and not f.startswith("IEndpoint"):
            path = os.path.join(root, f)
            with open(path, "r", encoding="utf-8") as file:
                content = file.read()
            
            # Remove IEndpoint interface
            content = re.sub(r'public class (\w+)\s*:\s*IEndpoint', r'public class \1', content)
            
            # Remove MapEndpoint block entirely
            content = re.sub(r'public void MapEndpoint\(IEndpointRouteBuilder app\)\s*\{[^\}]+\}', r'// MapEndpoint removed', content)

            # Map Http dependencies to MediatR and custom DI
            content = content.replace("using Microsoft.AspNetCore.Http;", "using MediatR;\nusing Application.Common.Models;\nusing Application.Common.Interfaces;")
            content = content.replace("using Microsoft.AspNetCore.Builder;", "")
            content = content.replace("using Microsoft.AspNetCore.Routing;", "")
            content = content.replace("using NATS.Client.JetStream;", "")
            content = content.replace("using Infrastructure.UnitofWork;", "")
            content = content.replace("IUnitOfWork", "IAttendanceDbContext")
            content = content.replace("INatsJSContext", "IEventPublisher")
            content = content.replace('using Application.GrpcService;', '')
            
            # Change Handler to MediatR IRequestHandler Handle method signature broadly
            # this is a bit hacky, let's find private async Task<IResult> Handler
            # and replace it.
            content = re.sub(
                r'private async Task<IResult> Handler\(([^\)]+)\)', 
                r'public class Handler : IRequestHandler<Command, Result>\n{\nprivate readonly IAttendanceDbContext db;\nprivate readonly IEventPublisher js;\nprivate readonly IIdentityService grpcClient;\nprivate readonly IEmailSender emailSender;\npublic Handler(IAttendanceDbContext db, IEventPublisher js, IIdentityService grpcClient, IEmailSender emailSender) { this.db = db; this.js = js; this.grpcClient = grpcClient; this.emailSender = emailSender; }\npublic async Task<Result> Handle(Command request, CancellationToken cancellationToken)\n{', 
                content
            )

            # Change IResult Returns
            content = re.sub(r'return Results\.BadRequest\((.+?)\);', r'return Result.Failure("Bad Request");', content)
            content = re.sub(r'return Results\.Created\((.+?)\);', r'return Result.Success();', content)
            content = re.sub(r'return Results\.Ok\((.+?)\);', r'return Result.Success();', content)
            content = re.sub(r'return Results\.NotFound\((.+?)\);', r'return Result.Failure("Not Found");', content)
            content = re.sub(r'return Results\.NoContent\(\);', r'return Result.Success();', content)

            with open(path, "w", encoding="utf-8") as file:
                file.write(content)

