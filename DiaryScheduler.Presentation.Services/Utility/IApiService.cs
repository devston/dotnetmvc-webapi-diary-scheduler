using DiaryScheduler.Presentation.Models.Base;
using System.Threading.Tasks;

namespace DiaryScheduler.Presentation.Services.Utility
{
    /// <summary>
    /// The interface for the api service.
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Used for calling a GET endpoint on the api asynchronously.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="callApiVm">The api settings object.</param>
        /// <returns>The return payload.</returns>
        Task<T> GetApiAsync<T>(CallApiViewModel callApiVm);

        /// <summary>
        /// Used for calling a POST endpoint on the api asynchronously.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="callApiVm">The api settings object.</param>
        /// <returns>The return payload.</returns>
        Task<T> PostApiAsync<T>(CallApiViewModel callApiVm);

        /// <summary>
        /// Used for calling a PUT endpoint on the api asynchronously.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="callApiVm">The api settings object.</param>
        /// <returns>The return payload.</returns>
        Task<T> PutApiAsync<T>(CallApiViewModel callApiVm);

        /// <summary>
        /// Used for calling a DELETE endpoint on the api asynchronously.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="callApiVm">The api settings object.</param>
        /// <returns>The return payload.</returns>
        Task<T> DeleteApiAsync<T>(CallApiViewModel callApiVm);
    }
}
