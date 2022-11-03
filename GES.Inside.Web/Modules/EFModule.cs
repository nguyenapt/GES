using Autofac;
using GES.Common.Factories;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Inside.Web.Modules
{

    public class EFModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterType(typeof(UnitOfWork<GesEntities>)).As(typeof(IUnitOfWork<GesEntities>)).InstancePerRequest();
            builder.RegisterType(typeof(UnitOfWork<GesRefreshDbContext>)).As(typeof(IUnitOfWork<GesRefreshDbContext>)).InstancePerRequest();

            builder.RegisterType<GesEntities>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GesRefreshDbContext>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork<GesEntities>>()
                .As<IUnitOfWork>()
                .WithParameter((pi, c) => pi.Name == "dbContext",
                    (pi, c) => c.Resolve<GesEntities>())
                .WithMetadata<Metadata>(m => m.For(am => am.Key, ContextKeys.OldContext))
                .InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork<GesRefreshDbContext>>()
                .As<IUnitOfWork>()
                .WithParameter((pi, c) => pi.Name == "dbContext",
                    (pi, c) => c.Resolve<GesRefreshDbContext>())
                .WithMetadata<Metadata>(m => m.For(am => am.Key, ContextKeys.NewContext))
                .InstancePerLifetimeScope();
        }
    }
}