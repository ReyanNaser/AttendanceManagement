using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Messages
{
    public class NatsStreamInitializer
    {
        public static async Task EnsureStreamExists(INatsJSContext js)
        {
            try
            {
                // Define the stream configuration
                var config = new StreamConfig(name: "EMPLOYEE_EVENTS", subjects: new[] { "user.>" });

                // Create or update the stream idempotently
                await js.CreateStreamAsync(config);
            }
            catch (Exception ex)
            {
                // Log warning (stream might already exist or server not ready)
                Console.WriteLine($"Stream setup warning: {ex.Message}");
            }
        }
    }
}
