using Autofac;
using DistancePrinterControl.API.Helpers;
using DistancePrinterControl.Database.Logic.Helpers.Sql;
using DistancePrinterControl.Database.Logic.Queries;
using DistancePrinterControl.Database.Logic.Queries.Interfaces;
using DistancePrinterControl.Database.Logic.ReadServices;
using DistancePrinterControl.Database.Logic.ReadServices.Interfaces;
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
            
            builder.Register(c => new PrinterQueries(c.Resolve<IConnectionStringHelper>()))
                .As<IPrinterQueries>()
                .InstancePerLifetimeScope();
            
            builder.Register(c => new PrinterReadService(c.Resolve<IPrinterQueries>()))
                .As<IPrinterReadService>()
                .InstancePerLifetimeScope();
            
            builder.Register(c => new CredsHelper(c.Resolve<IConfiguration>()))
                .As<ICredsHelper>()
                .InstancePerLifetimeScope();
        }
        
        public static ContainerBuilder ContainerBuilderConfig(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionStringHelper>();
            builder.RegisterType<PrinterQueries>();
            builder.RegisterType<PrinterReadService>();
            
            return builder;
        }
    }
}