using Autofac;
using DistancePrinterControl.Database.Logic.Helpers.Sql;
using Microsoft.Extensions.Configuration;

namespace DistancePrinterControl.API.configs
{
    public class DistancePrinterControlAutofacConfig : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ConnectionStringHelper(c.Resolve<IConfiguration>()))
                .As<IConnectionStringHelper>()
                .InstancePerLifetimeScope();
        }
        
        public static ContainerBuilder ContainerBuilderConfig(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionStringHelper>();
            
            
            return builder;
        }
    }
}