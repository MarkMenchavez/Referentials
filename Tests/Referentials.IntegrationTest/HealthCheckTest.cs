namespace Referentials.IntegrationTest.Controllers;

using System.Net;
using Xunit;
using Xunit.Abstractions;

public class HealthCheckTest : CustomWebApplicationFactory<Program>
{
    public HealthCheckTest(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task GetStatus_Default_Returns200OkAsync()
    {
        var response = await this.CreateClient().GetAsync(new Uri("/status", UriKind.Relative)).ConfigureAwait(false);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetStatusSelf_Default_Returns200OkAsync()
    {
        var response = await this.CreateClient().GetAsync(new Uri("/status/self", UriKind.Relative)).ConfigureAwait(false);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
