using DiaryScheduler.ScheduleManagement.Core.Interfaces;
using DiaryScheduler.ScheduleManagement.Data.Repositories;
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

                    x.For<IScheduleRepository>().Use<ScheduleRepository>();
                });

                moreInitialization?.Invoke(x);
            });
        }
    }
}
