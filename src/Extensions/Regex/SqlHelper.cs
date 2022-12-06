using System;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Threading;

namespace Extensions
{
    public static class SqlHelper
    {
        public static SqlConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public static SqlCommand CreateCommand(string connectionString, string commandText, CommandType commandType, params SqlParameter[] args)
        {
            var sqlCommand = new SqlCommand(commandText)
            {
                CommandType = commandType,
                Connection = CreateConnection(connectionString)
            };

            sqlCommand.Parameters.AddRange(args);
            return sqlCommand;
        }

        public static async Task<object> ExecuteScalar(SqlCommand command, CancellationToken cancellation)
        {
            object result = null;

            await TryRun(command,
                async cmd => result = await cmd.ExecuteScalarAsync(cancellation));

            return result;
        }

        public static async Task ExecuteNoneQuery(SqlCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.Connection.State != ConnectionState.Open)
                {
                    await command.Connection.OpenAsync(cancellationToken);
                }

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            finally
            {
                await command.Connection.CloseAsync();
                await command.Connection.DisposeAsync();
                await command.DisposeAsync();
            }
        }

        public static async Task ExecuteReader(SqlCommand command, Action<SqlDataReader> readerHandler, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (command.Connection.State != ConnectionState.Open)
                {
                    await command.Connection.OpenAsync(cancellationToken);
                }

                var reader = await command.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    readerHandler(reader);
                }

                await reader.DisposeAsync();
            }
            finally
            {
                await command.Connection.CloseAsync();
                await command.Connection.DisposeAsync();
                await command.DisposeAsync();
            }
        }

        private static async Task TryRun(SqlCommand command, Func<SqlCommand, Task> func)
        {
            try
            {
                if (command.Connection.State != ConnectionState.Open)
                {
                    await command.Connection.OpenAsync();
                }

                await func(command);
            }
            finally
            {
                await command.Connection.CloseAsync();
                await command.Connection.DisposeAsync();
                await command.DisposeAsync();
            }
        }
    }
}
