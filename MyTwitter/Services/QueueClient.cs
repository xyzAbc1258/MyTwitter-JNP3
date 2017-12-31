using System;
using MassTransit;
using MyTwitter.Common;
using MyTwitter.Models;

namespace MyTwitter.Services
{
    public class QueueClient : IQueueClient
    {
        private readonly IBusControl _busControl;
        private bool _started = false;
        public QueueClient(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async void Create(Post post)
        {
            var endp = await _busControl.GetSendEndpoint(new Uri("rabbitmq://queueserver/messagesA"));
            await endp.Send(new CreateMessageDTO(){Message = post.Message, UserId = post.ApplicationUser.Id});
        }

        public async void Update(Post post)
        {
            var endp = await _busControl.GetSendEndpoint(new Uri("rabbitmq://queueserver/messagesU"));
            await endp.Send(new UpdateMessageDTO() {Message = post.Message, PostId = post.Id});
        }
        
    }
}
