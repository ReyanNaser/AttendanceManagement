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
            
                
                var config = new StreamConfig(name: "EMPLOYEE_EVENTS_V2", subjects: new[] { "user.>" });

               
                await js.CreateStreamAsync(config);
            
        }
    }
}
