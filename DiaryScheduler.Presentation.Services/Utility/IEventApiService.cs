using System.Threading.Tasks;

namespace DiaryScheduler.Presentation.Services.Utility;

/// <summary>
/// This service is used to call the event api.
/// </summary>
public interface IEventApiService
{
    /// <summary>
    /// Used for calling a GET endpoint on the event api asynchronously.
    /// </summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="endpoint">The endpoint to call.</param>
    /// <param name="queryParams">Parameters to send via query string.</param>
    /// <returns>The returned payload.</returns>
    Task<T> GetApiAsync<T>(string endpoint, object queryParams = null);

    /// <summary>
    /// Used for calling a POST endpoint on the event api asynchronously.
    /// </summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="endpoint">The endpoint to call.</param>
    /// <param name="bodyModel">A model to send via body in the request.</param>
    /// <returns>The returned payload.</returns>
    Task<T> PostApiAsync<T>(string endpoint, object bodyModel);

    /// <summary>
    /// Used for calling a PUT endpoint on the event api asynchronously.
    /// </summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="endpoint">The endpoint to call.</param>
    /// <param name="bodyModel">A model to send via body in the request.</param>
    /// <returns>The returned payload.</returns>
    Task<T> PutApiAsync<T>(string endpoint, object bodyModel);

    /// <summary>
    /// Used for calling a DELETE endpoint on the event api asynchronously.
    /// </summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="endpoint">The endpoint to call.</param>
    /// <param name="queryParams">Parameters to send via query string.</param>
    /// <returns>The returned payload.</returns>
    Task<T> DeleteApiAsync<T>(string endpoint, object queryParams = null);
}
