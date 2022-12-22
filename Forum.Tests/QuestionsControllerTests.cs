using FluentAssertions;
using Forum.WebAPI.Dto_s;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Forum.Tests;

/// <summary>
/// Tests for Questions Controller. 
/// Tests work fine with current local db state.
/// </summary>
public class QuestionsControllerTests
{
    private HttpClient _client_Admin;
    private HttpClient _client_User;

    public QuestionsControllerTests()
    {
        _client_Admin = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                services.AddMvc(option => option.Filters.Add(new FakeUserWithAdministratorPermissionsFilter()));
            });
        }).CreateClient();

        _client_User = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                });
            }).CreateClient();
    }

    #region GetAll
    [Theory]
    [InlineData("PageNumber=1&PageSize=15&SortBy=Topic&SortOrder=0")]
    [InlineData("PageNumber=1&PageSize=10&SortBy=Topic&SortOrder=1")]
    [InlineData("PageNumber=1&PageSize=5&SortBy=Topic&SortOrder=0")]
    [InlineData("PageNumber=1&PageSize=5&SortBy=Date&SortOrder=0")]
    public async Task GetAll_WithQueryParameters_ReturnsOkResult(string queryParameters)
    {
        // Act
        var response = await _client_User.GetAsync("/Questions/GetAll?" + queryParameters);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("PageNumber=1&PageSize=4&SortBy=Topic&SortOrder=0")]
    [InlineData("PageNumber=1&PageSize=10&SortBy=Question&SortOrder=1")]
    [InlineData("PageNumber=1&PageSize=5&SortBy=Topic&SortOrder=2")]
    public async Task GetAll_WithQueryParameters_ReturnsBadRequestResult(string queryParameters)
    {
        // Act
        var response = await _client_User.GetAsync("/Questions/GetAll?" + queryParameters);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
    #endregion

    #region Get
    [Theory]
    [InlineData(3)]
    [InlineData(2)]
    public async Task Get_WithQueryParameters_ReturnsOkResult(int id)
    {
        // Act
        var response = await _client_User.GetAsync("/Questions/Get/" + id);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Get_WithQueryParameters_ReturnsNotFoundResult(int id)
    {
        // Act
        var response = await _client_User.GetAsync("/Questions/Get/" + id);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
    #endregion

    #region Create
    [Fact]
    public async Task CreateQuestion_WithValidModel_ReturnsCreatedStatus()
    {
        // Arrange
        var model = new CreateQuestionDto()
        {
            Topic = "Test",
            Content = "Test Content"
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client_User.PostAsync("/Questions", httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateQuestion_WithInvalidModel_ReturnsBadRequestStatus()
    {
        // Arrange
        var model = new CreateQuestionDto()
        {
            Topic = null,
            Content = "Test Content"
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client_Admin.PostAsync("/Questions", httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
    #endregion

    #region Put
    [Theory]
    [InlineData(43)]
    [InlineData(44)]
    public async Task PutQuestion_WithValidModel_ReturnsOkStatus(int id)
    {
        // Arrange
        var model = new UpdateQuestionDto()
        {
            Id = id,
            Topic = "Test",
            Content = "Test Content"
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client_Admin.PutAsync("/Questions/" + id, httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(43)]
    [InlineData(44)]
    public async Task PutQuestion_WithValidModelButInvalidRequest_ReturnsNotFoundStatus(int id)
    {
        // Arrange
        var model = new UpdateQuestionDto()
        {
            Id = id,
            Topic = "Test",
            Content = "Test Content"
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client_Admin.PutAsync("/Questions/" + id + 1, httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task PutQuestion_WithValidModelThatDoesntExist_ReturnsNotFoundStatus(int id)
    {
        // Arrange
        var model = new UpdateQuestionDto()
        {
            Id = id,
            Topic = "Test",
            Content = "Test Content"
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client_Admin.PutAsync("/Questions/" + id, httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    public async Task PutQuestion_WithValidModelThatHasAnotherOwner_ReturnsForbiddenStatus(int id)
    {
        // Arrange
        var model = new UpdateQuestionDto()
        {
            Id = id,
            Topic = "Test",
            Content = "Test Content"
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client_User.PutAsync("/Questions/" + id, httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }
    #endregion

    #region Delete
    [Theory]
    [InlineData(43)]
    [InlineData(44)]
    public async Task DeleteQuestion_ForAdmin_ReturnsNoContentStatus(int id)
    {
        // Act
        var response = await _client_Admin.DeleteAsync("/Questions/" + id);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DeleteQuestion_ForQuestionThatDoesntExist_ReturnsNotFoundStatus(int id)
    {
        // Act
        var response = await _client_User.DeleteAsync("/Questions/" + id);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(7)]
    public async Task DeleteQuestion_ForQuestionWithAnotherOwner_ReturnsForbiddenStatus(int id)
    {
        // Act
        var response = await _client_User.DeleteAsync("/Questions/" + id);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }
    #endregion
}
