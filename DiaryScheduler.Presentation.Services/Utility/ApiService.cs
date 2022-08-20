using DiaryScheduler.Presentation.Models.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DiaryScheduler.Presentation.Services.Utility;

/// <summary>
/// The implementation of the <see cref="IApiService"/>.
/// </summary>
public class ApiService : IApiService
{
    public async Task<T> GetApiAsync<T>(CallApiViewModel callApiVm)
    {
        try
        {
            // Call the web api.
            using (var client = new HttpClient())
            {
                // Check if we need to add authentication.
                if (callApiVm.RequiresAuthentication)
                {
                    var accessToken = await GetClientTokenAsync(callApiVm);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                var url = callApiVm.ApiUrl;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Check if we need to append a query string.
                if (callApiVm.QueryParams != null)
                {
                    var queryString = GetQueryString(callApiVm.QueryParams);
                    callApiVm.Endpoint = callApiVm.Endpoint + "?" + queryString;
                }

                // Call the service.
                client.Timeout = TimeSpan.FromMinutes(20);
                client.BaseAddress = new Uri(url);
                var response = await client.GetAsync(callApiVm.Endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{callApiVm.Endpoint}: API did not response with a 200 [OK] but a {response.StatusCode} instead. Content: {response.Content}");
                }

                var receiveStream = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(receiveStream);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<T> PostApiAsync<T>(CallApiViewModel callApiVm)
    {
        try
        {
            // Call the web api.
            using (var client = new HttpClient())
            {
                // Check if we need to add authentication.
                if (callApiVm.RequiresAuthentication)
                {
                    var accessToken = await GetClientTokenAsync(callApiVm);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                var url = callApiVm.ApiUrl;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Sort out the body.
                var message = JsonConvert.SerializeObject(callApiVm.Body);
                var content = new StringContent(message, Encoding.UTF8, "application/json");

                // Call the service.
                client.Timeout = TimeSpan.FromMinutes(20);
                client.BaseAddress = new Uri(url);
                var response = await client.PostAsync(callApiVm.Endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{callApiVm.Endpoint}: API did not response with a 200 [OK] but a {response.StatusCode} instead. Content: {response.Content}");
                }

                var receiveStream = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(receiveStream);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<T> PutApiAsync<T>(CallApiViewModel callApiVm)
    {
        try
        {
            // Call the web api.
            using (var client = new HttpClient())
            {
                // Check if we need to add authentication.
                if (callApiVm.RequiresAuthentication)
                {
                    var accessToken = await GetClientTokenAsync(callApiVm);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                var url = callApiVm.ApiUrl;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Sort out the body.
                var message = JsonConvert.SerializeObject(callApiVm.Body);
                var content = new StringContent(message, Encoding.UTF8, "application/json");

                // Call the service.
                client.Timeout = TimeSpan.FromMinutes(20);
                client.BaseAddress = new Uri(url);
                var response = await client.PutAsync(callApiVm.Endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{callApiVm.Endpoint}: API did not response with a 200 [OK] but a {response.StatusCode} instead. Content: {response.Content}");
                }

                var receiveStream = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(receiveStream);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<T> DeleteApiAsync<T>(CallApiViewModel callApiVm)
    {
        try
        {
            // Call the web api.
            using (var client = new HttpClient())
            {
                // Check if we need to add authentication.
                if (callApiVm.RequiresAuthentication)
                {
                    var accessToken = await GetClientTokenAsync(callApiVm);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                var url = callApiVm.ApiUrl;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Check if we need to append a query string.
                if (callApiVm.QueryParams != null)
                {
                    var queryString = GetQueryString(callApiVm.QueryParams);
                    callApiVm.Endpoint = callApiVm.Endpoint + "?" + queryString;
                }

                // Call the service.
                client.Timeout = TimeSpan.FromMinutes(20);
                client.BaseAddress = new Uri(url);
                var response = await client.DeleteAsync(callApiVm.Endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{callApiVm.Endpoint}: API did not response with a 200 [OK] but a {response.StatusCode} instead. Content: {response.Content}");
                }

                var receiveStream = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(receiveStream);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region Helpers

    /// <summary>
    /// Get the client token required to call the api asynchronously.
    /// </summary>
    /// <param name="callApiVm">The call api model.</param>
    /// <returns>The access token.</returns>
    private async Task<string> GetClientTokenAsync(CallApiViewModel callApiVm)
    {
        try
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, callApiVm.AuthApiUrl);
                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", callApiVm.ClientId },
                    { "client_secret", callApiVm.ClientSecret },
                    { "grant_type", callApiVm.GrantType },
                    { "scope", callApiVm.Scope }
                });

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
                var token = payload.Value<string>("access_token");
                return token;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Get the query string from an object.
    /// </summary>
    /// <param name="obj">The query string object.</param>
    /// <returns>The configured query string.</returns>
    private string GetQueryString(object obj)
    {
        var properties = obj.GetType().GetProperties()
                         .Where(x => x.GetValue(obj, null) != null)
                         .Select(x => x.Name + "=" + HttpUtility.UrlEncode(x.GetValue(obj, null).ToString()));
        return string.Join("&", properties.ToArray());
    }

    #endregion
}
