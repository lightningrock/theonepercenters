using Ninject.Modules;
using System.Data.Entity;
using TheOnePercents.Domain;
using TheOnePercents.Model;
using TheOnePercents.Repository;

namespace TheOnePercents.Infrastructure
{
    public class InjectControllerFactory
    {
        public class TopsDependencies : NinjectModule
        {

            public override void Load()
            {
                Bind<DbContext>().To<theonepercentersEntities1>();
                Bind(typeof(IRepositoryBase<>)).To(typeof(RepositoryBase<>));

                Bind<IEmailService>().To<EmailService>();
                Bind<IOnboardingNewMember>().To<OnboardingNewMember>();
            }
        }
    }
}
