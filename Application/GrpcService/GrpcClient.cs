using AuthServiceProvider.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using static AuthServiceProvider.Protos.AuthService;
using static AuthServiceProvider.Protos.RoleService;

namespace Application.GrpcService
{
    public class GrpcClient
    {
        private readonly ILogger<GrpcClient> _logger;
        private readonly AuthServiceClient _authclient;
        private readonly RoleServiceClient _roleclient;

        public GrpcClient(ILogger<GrpcClient> logger, AuthServiceClient authclient, RoleServiceClient roleclient)
        {
            _logger = logger;
            _authclient = authclient;
            _roleclient = roleclient;
        }

        public async Task<(bool Success, string Message, string? UserId)> CreateUserAsync(CreateUserRequest requestGrpc, CancellationToken token)
        {
            try
            {
                var result = await _authclient.CreateUserAsync(requestGrpc, cancellationToken: token);

                return result.Success
                    ? (true, result.Message, result.UserId)
                    : (false, result.Message, null);
            }
            catch (RpcException ex)
            {
                string message = $"gRPC Error: {ex.Status.Detail}";
                _logger.LogError("gRPC Exception: {message}", message);
                return (false, message, null);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                _logger.LogError("General Exception: {message}", message);
                return (false, message, null);
            }
        }

        public async Task<(bool Success, string Message)> PromoteToManagerAsync(PromotionRequest requestGrpc, CancellationToken token)
        {
            try
            {
                var result = await _roleclient.PromoteToManagerAsync(requestGrpc, cancellationToken: token);

                return result.Success
                    ? (true, result.Message)
                    : (false, result.Message);
            }
            catch (RpcException ex)
            {
                string message = $"gRPC Error: {ex.Status.Detail}";
                _logger.LogError("gRPC Exception: {message}", message);
                return (false, message);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                _logger.LogError("General Exception: {message}", message);
                return (false, message);
            }
        }
    }
}
