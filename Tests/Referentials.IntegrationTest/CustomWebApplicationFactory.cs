namespace Referentials.IntegrationTest;

using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Referentials.Options;
using Referentials.Repositories;
using Referentials.Services;
using Serilog;
using Serilog.Events;
using Xunit.Abstractions;

public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
    where TEntryPoint : class
{
    public CustomWebApplicationFactory(ITestOutputHelper testOutputHelper)
    {
        this.ClientOptions.AllowAutoRedirect = false;
        this.ClientOptions.BaseAddress = new Uri("https://localhost");

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Debug(formatProvider: CultureInfo.InvariantCulture)
            .WriteTo.TestOutput(testOutputHelper, LogEventLevel.Verbose, formatProvider: CultureInfo.InvariantCulture)
            .CreateLogger();
    }

    public ApplicationOptions ApplicationOptions { get; private set; } = default!;

    public Mock<ICarRepository> CarRepositoryMock { get; } = new Mock<ICarRepository>(MockBehavior.Strict);

    public Mock<IClockService> ClockServiceMock { get; } = new Mock<IClockService>(MockBehavior.Strict);

    public void VerifyAllMocks() => Mock.VerifyAll(this.CarRepositoryMock, this.ClockServiceMock);

    protected override void ConfigureClient(HttpClient client)
    {
        using (var serviceScope = this.Services.CreateScope())
        {
            var serviceProvider = serviceScope.ServiceProvider;
            this.ApplicationOptions = serviceProvider.GetRequiredService<ApplicationOptions>();
        }

        base.ConfigureClient(client);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .ConfigureServices(this.ConfigureServices)
            .UseEnvironment(Constants.EnvironmentName.Test);
    }

    protected virtual void ConfigureServices(IServiceCollection services) =>
        services
            .AddDistributedMemoryCache()
            .AddSingleton(this.CarRepositoryMock.Object)
            .AddSingleton(this.ClockServiceMock.Object);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.VerifyAllMocks();
        }

        base.Dispose(disposing);
    }
}
