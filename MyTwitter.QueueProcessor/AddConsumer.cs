using System;
using System.Data;
using System.Threading.Tasks;
using MassTransit;
using MyTwitter.Common;
namespace MyTwitter.QueueProcessor
{
    public class AddConsumer : IConsumer<CreateMessageDTO>
    {
        private readonly Func<IDbConnection> _connectionFactory;

        public AddConsumer(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task Consume(ConsumeContext<CreateMessageDTO> context)
        {
            return Task.Run(() =>
            {
                using (var connection = _connectionFactory())
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"INSERT INTO Posts(IdApplicationUser, Message, DateCreated, DateUpdated)
                                              Values(@userId, @msg, @dt, @dt)";
                    command.AddParameter("@userId", DbType.Int32, context.Message.UserId);
                    command.AddParameter("@dt", DbType.DateTime2, DateTime.Now);
                    command.AddParameter("@msg", DbType.String, context.Message.Message);
                    command.ExecuteNonQuery();
                }
            });
        }
    }
}
