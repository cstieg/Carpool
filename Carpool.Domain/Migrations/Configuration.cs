namespace Carpool.Domain.Migrations
{
    using Carpool.Domain.Repository;
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<EntitiesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EntitiesContext context)
        {

        }
    }
}
