using System;
using System.Reflection;
using DbUp;

namespace Ubora.Domain.Infrastructure
{
    public class DomainMigrator
    {
        /// Uses DbUp - <see cref="http://dbup.readthedocs.io/en/latest/"/>>
        public void MigrateDomain(string connectionString)
        {
            EnsureDatabase.For.PostgresqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .WithVariablesDisabled()
                    .Build();

            var upgradeResult = upgrader.PerformUpgrade();
            if (!upgradeResult.Successful)
            {
                throw new Exception("Ubora.Domain database migration did not succeed.", upgradeResult.Error);
            }
        }
    }
}