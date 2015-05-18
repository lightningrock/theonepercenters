using Ninject;

namespace TheOnePercents.Infrastructure
{
    public static class RepositoryDI
    {
        public static T Resolve<T>()
        {
            return Container.TryGet<T>();
        }

        private static IKernel _container;

        private static IKernel Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new StandardKernel(new InjectControllerFactory.TopsDependencies());
                }
                return _container;
            }
        }
    }
}
