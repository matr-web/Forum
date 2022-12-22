using FluentAssertions;
using Forum.WebAPI.Dto_s;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Forum.Tests;


/// <summary>
/// Tests for Ratings Controller. 
/// Tests work fine with current local db state.
/// </summary>
public class RatingsControllerTests
{
    private HttpClient _client;

    public RatingsControllerTests()
	{
        _client = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.AddMvc(option => option.Filters.Add(new FakeUserWithAdministratorPermissionsFilter()));
                });
            }).CreateClient();
    }

    [Fact]
    public async Task CreateRating_WithValidModel_ReturnsOkStatus()
    {
        // Arrange
        var model = new CreateRatingDto()
        {
            Value = 1,
            QuestionId = 8
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client.PostAsync("/Ratings", httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
