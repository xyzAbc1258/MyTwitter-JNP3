using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Internals.Extensions;

namespace MyTwitter.QueueProcessor
{
    public static class DbCommandExtensions
    {
        public static void AddParameter<T>(this IDbCommand command, string parameterName, DbType dbType, T value)
        {
            var param = command.CreateParameter();
            param.ParameterName = parameterName;
            param.DbType = dbType;
            param.Value = value;
            command.Parameters.Add(param);
        }
    }
}
