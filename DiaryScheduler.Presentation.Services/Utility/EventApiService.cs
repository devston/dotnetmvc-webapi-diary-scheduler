using DiaryScheduler.Presentation.Models.Base;
using DiaryScheduler.Presentation.Services.Configuration;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DiaryScheduler.Presentation.Services.Utility
{
    /// <summary>
    /// The implementation of the <see cref="IEventApiService"/>.
    /// </summary>
    public class EventApiService : IEventApiService
    {
        private readonly IConfiguration _configuration;
        private readonly IApiService _apiService;

        public EventApiService(
            IConfiguration configuration,
            IApiService apiService)
        {
            _configuration = configuration;
            _apiService = apiService;
        }

        public async Task<T> GetApiAsync<T>(string endpoint, object queryParams = null)
        {
            var callApiVm = ConstructCallApiModel(endpoint, queryParams: queryParams);
            return await _apiService.GetApiAsync<T>(callApiVm);
        }

        public async Task<T> PostApiAsync<T>(string endpoint, object bodyModel)
        {
            var callApiVm = ConstructCallApiModel(endpoint, bodyModel);
            return await _apiService.PostApiAsync<T>(callApiVm);
        }

        public async Task<T> PutApiAsync<T>(string endpoint, object bodyModel)
        {
            var callApiVm = ConstructCallApiModel(endpoint, bodyModel);
            return await _apiService.PutApiAsync<T>(callApiVm);
        }

        public async Task<T> DeleteApiAsync<T>(string endpoint, object queryParams = null)
        {
            var callApiVm = ConstructCallApiModel(endpoint, queryParams: queryParams);
            return await _apiService.DeleteApiAsync<T>(callApiVm);
        }

        /// <summary>
        /// Construct the <see cref="CallApiViewModel"/>.
        /// </summary>
        /// <param name="endpoint">The endpoint to call.</param>
        /// <param name="bodyModel">The model to send in the body.</param>
        /// <param name="queryParams">The query string parameters.</param>
        /// <returns>The constructed <see cref="CallApiViewModel"/>.</returns>
        private CallApiViewModel ConstructCallApiModel(string endpoint, object bodyModel = null, object queryParams = null)
        {
            var apiConfig = _configuration.GetSection(EventApiConfig.SectionName).Get<EventApiConfig>();
            var callApiVm = new CallApiViewModel();
            callApiVm.ApiUrl = apiConfig.Url;
            callApiVm.Endpoint = endpoint;
            callApiVm.Body = bodyModel;
            callApiVm.QueryParams = queryParams;
            return callApiVm;
        }
    }
}
