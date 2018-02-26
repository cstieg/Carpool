namespace Carpool.Services.Tests.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Carpool.Services.Tests.TestContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Carpool.Services.Tests.TestContext context)
        {
        }
    }
}
