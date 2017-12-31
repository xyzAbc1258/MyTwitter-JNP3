using System.Threading.Tasks;
using MassTransit;
using MyTwitter.Common;
namespace MyTwitter.QueueProcessor
{
    public class AddConsumer : IConsumer<CreateMessageDTO>
    {
        public Task Consume(ConsumeContext<CreateMessageDTO> context)
        {
            return Task.CompletedTask;
        }
    }
}
