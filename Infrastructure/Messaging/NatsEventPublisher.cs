using Application.Common.Interfaces;
using NATS.Client.JetStream;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Messaging;

public class NatsEventPublisher : IEventPublisher
{
    private readonly INatsJSContext _js;

    public NatsEventPublisher(INatsJSContext js)
    {
        _js = js;
    }

    public async Task PublishAsync<T>(string subject, T @event, CancellationToken cancellationToken = default)
    {
        var ack = await _js.PublishAsync(subject, @event, cancellationToken: cancellationToken);
        ack.EnsureSuccess();
    }
}
