using StructureMap;
using System;

namespace DiaryScheduler.DependencyResolution
{
    public static partial class IoC
    {
        public static IContainer Initialize(Action<ConfigurationExpression> moreInitialization)
        {
            return new Container(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                moreInitialization?.Invoke(x);
            });
        }
    }
}
