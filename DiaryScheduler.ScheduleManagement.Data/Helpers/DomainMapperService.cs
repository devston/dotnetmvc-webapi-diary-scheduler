using AutoMapper;
using DiaryScheduler.Data.Models;
using DiaryScheduler.ScheduleManagement.Core.Models;

namespace DiaryScheduler.ScheduleManagement.Data.Helpers
{
    public class DomainMapperService
    {
        private static IMapper _mapper;

        static DomainMapperService()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CalendarEntry, CalEntry>()
                   .ReverseMap()
                   .ForMember(x => x.AspNetUser, opt => opt.Ignore());
            });

            _mapper = config.CreateMapper();
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        internal T Map<T>(object objectToBeMapped)
        {
            return _mapper.Map<T>(objectToBeMapped);
        }
    }
}
