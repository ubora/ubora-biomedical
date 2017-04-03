using Marten;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ubora.Domain.Tests
{
    public class ConnectionSource : ConnectionFactory
    {
        public static readonly string ConnectionString = Environment.GetEnvironmentVariable("marten-testing-database");

        static ConnectionSource()
        {
            if (ConnectionString.IsEmpty())
                throw new Exception(
                    "You need to set the connection string for your local Postgresql database in the environment variable 'marten-testing-database'");
        }


        public ConnectionSource() : base(ConnectionString)
        {
            
        }
    }
}
