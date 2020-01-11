using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MissionControl.Services;
using MissionControl.Services.Common;
using MissionControl.Services.Factories;

namespace MissionControl.Server
{
    public class AutofacModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            // The generic ILogger<TCategoryName> service was added to the ServiceCollection by ASP.NET Core.
            // It was then registered with Autofac using the Populate method. All of this starts
            // with the services.AddAutofac() that happens in Program and registers Autofac
            // as the service provider.

            builder.Register(context => new ApplicationDbContext(context.Resolve<DbContextOptions<ApplicationDbContext>>()))
                     .As<IDbContext>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<VendorService>().As<IVendorService>().InstancePerLifetimeScope();
            builder.RegisterType<PurchaseService>().As<IPurchaseService>().InstancePerLifetimeScope();
            builder.RegisterType<PdfService>().As<IPdfService>().InstancePerLifetimeScope();
            builder.RegisterType<PurchaseModelFactory>().As<IPurchaseModelFactory>().InstancePerLifetimeScope();
          //  builder.RegisterType<AutoMapping>().As<IMapper>().InstancePerLifetimeScope();
        }
    }
}
