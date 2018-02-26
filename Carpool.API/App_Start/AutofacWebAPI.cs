using Autofac;
using Autofac.Integration.WebApi;
using Carpool.Domain.Models;
using Carpool.Domain.Repository;
using Carpool.Domain.Services;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;

namespace Carpool.API.App_Start
{
    public class AutofacWebAPI
    {
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // message handlers
            config.MessageHandlers.Add(new RequireHttpsMessageHandler());
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            // registration goes here
            // EF DbContext
            builder.RegisterType<EntitiesContext>().As<DbContext>().InstancePerApiRequest();

            // repositories
            builder.RegisterGeneric(typeof(EntityRepository<>)).As(typeof(IEntityRepository<>)).InstancePerApiRequest();

            // services
            builder.RegisterType<MembershipService>().As<MembershipService>().InstancePerApiRequest();


            return builder.Build();
        }
    }
}