using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;

namespace MyTwitter.QueueProcessor
{
    public class UpdateConsumer : IConsumer<UpdateConsumer>
    {
        public Task Consume(ConsumeContext<UpdateConsumer> context)
        {
            return Task.CompletedTask;
        }
    }
}
