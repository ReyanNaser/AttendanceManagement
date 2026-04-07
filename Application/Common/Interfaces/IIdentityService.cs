using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;

public record CreateUserRequestDto(string FirstName, string LastName, string Email, string Role);
public record PromoteUserRequestDto(string Email, string Role);

public interface IIdentityService
{
    Task<(bool Success, string Message)> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);
    Task<(bool Success, string Message)> PromoteToManagerAsync(PromoteUserRequestDto request, CancellationToken cancellationToken = default);
}
