using Carpool.Services.Tests.Repositories;

namespace Carpool.Services.Tests.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<CarpoolTestContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CarpoolTestContext context)
        {
        }
    }
}
