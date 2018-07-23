using StructureMap;
using System;

namespace DiaryScheduler.DependencyResolution
{
    public static partial class IoC
    {
        private static IContainer _inst;
        public static IContainer MainContainer
        {
            get
            {
                return _inst ?? (_inst = Initialize(x => { }));
            }
        }

        public static void InitializeMainContainer(Action<ConfigurationExpression> additionalConfig)
        {
            if (_inst != null)
            {
                throw new InvalidOperationException("Cannot call initialize twice.");
            }
                
            _inst = Initialize(additionalConfig);
        }

        public static IContainer GetMainContainer()
        {
            return MainContainer;
        }
    }
}
