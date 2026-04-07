using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(string subject, T @event, CancellationToken cancellationToken = default);
}
