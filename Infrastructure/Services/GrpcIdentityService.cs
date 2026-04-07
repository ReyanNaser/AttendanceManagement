using Application.Common.Interfaces;
using Application.GrpcService;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class GrpcIdentityService : IIdentityService
{
    private readonly GrpcClient _grpcClient;

    public GrpcIdentityService(GrpcClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    public async Task<(bool Success, string Message)> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var grpcRequest = new AuthServiceProvider.Protos.CreateUserRequest
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Role = request.Role
        };

        var result = await _grpcClient.CreateUserAsync(grpcRequest, cancellationToken);
        return (result.Success, result.Message);
    }

    public async Task<(bool Success, string Message)> PromoteToManagerAsync(PromoteUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var grpcRequest = new AuthServiceProvider.Protos.PromotionRequest
        {
            Email = request.Email,
            Role = request.Role
        };

        var result = await _grpcClient.PromoteToManagerAsync(grpcRequest, cancellationToken);
        return (result.Success, result.Message);
    }
}
