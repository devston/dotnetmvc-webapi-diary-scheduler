using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace DiaryScheduler.Api.Integration.Tests.Configuration;

public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // This is called after the `ConfigureServices` from Startup
        // which allows us to overwrite the DI with the mocked instances if we need to.
        builder.ConfigureTestServices(services =>
        {
            Utilities.ConfigureServices(services);
        });
    }
}
