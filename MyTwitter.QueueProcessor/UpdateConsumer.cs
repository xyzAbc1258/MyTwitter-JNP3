using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Internals.Extensions;
using MyTwitter.Common;

namespace MyTwitter.QueueProcessor
{
    public class UpdateConsumer : IConsumer<UpdateMessageDTO>
    {
        private readonly Func<IDbConnection> _connectionFactory;

        public UpdateConsumer(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task Consume(ConsumeContext<UpdateMessageDTO> context)
        {
            return Task.Run(() =>
            {
                using (var connection = _connectionFactory())
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "Update Posts Set Message = @msg, DateUpdated = @dt Where Id = @id";
                    command.AddParameter("@msg",DbType.String, context.Message.Message);
                    command.AddParameter("@id",DbType.Int32, context.Message.PostId);
                    command.AddParameter("@dt",DbType.DateTime2, DateTime.Now);
                    command.ExecuteNonQuery();
                }
            });
        }
    }
}
