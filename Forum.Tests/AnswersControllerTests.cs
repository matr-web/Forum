using FluentAssertions;
using Forum.WebAPI.Dto_s;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Forum.Tests;

/// <summary>
/// Tests for Answers Controller. 
/// Tests work fine with current local db state.
/// </summary>
public class AnswersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient _client_Admin;
    private HttpClient _client_User;

    public AnswersControllerTests(WebApplicationFactory<Program> factory)
    {
        _client_Admin = factory
           .WithWebHostBuilder(builder =>
           {
               builder.ConfigureServices(services =>
               {
                   services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                   services.AddMvc(option => option.Filters.Add(new FakeUserWithAdministratorPermissionsFilter()));
               });
           }).CreateClient();

        _client_User = factory
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
    [Fact]
    public async Task GetAll_ReturnsOkResult()
    {
        // Act
        var response = await _client_User.GetAsync("/Answers/GetAll");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    #endregion

    #region Get
    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Get_WithQueryParameters_ReturnsOkResult(int id)
    {
        // Act
        var response = await _client_User.GetAsync("/Answers/Get/" + id);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Get_WithInvalidQueryParameters_ReturnsNotFoundResult(int id)
    {
        // Act
        var response = await _client_User.GetAsync("/Answers/Get/" + id);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
    #endregion

    #region Create
    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    public async Task CreateAnswer_WithValidModel_ReturnsCreatedStatus(int questionId)
    {
        // Arrange
        var model = new CreateAnswerDto()
        {
            Content = "Test Content"
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client_User.PostAsync("/Answers/" + questionId, httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    public async Task CreateAnswer_WithInvalidModel_ReturnsBadRequestStatus(int questionId)
    {
        // Arrange
        var model = new CreateAnswerDto()
        {
            Content = null
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client_User.PostAsync("/Answers/" + questionId, httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    public async Task CreateAnswer_WithInvalidClient_ReturnsUnauthorizedStatus(int questionId)
    {
        // Arrange
        var model = new CreateAnswerDto()
        {
            Content = "Test Content"
        };

        var _client = new WebApplicationFactory<Program>().CreateClient();

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client.PostAsync("/Answers/" + questionId, httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
    #endregion

    #region Put
    [Theory]
    [InlineData(43)]
    [InlineData(44)]
    public async Task PutAnswer_WithValidModel_ReturnsOkStatus(int id)
    {
        // Arrange
        var model = new UpdateAnswerDto()
        {
            Id = id,
            Content = "Test Content"
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client_Admin.PutAsync("/Answers/" + id, httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(43)]
    [InlineData(44)]
    public async Task PutQuestion_WithValidModelButInvalidRequest_ReturnsNotFoundStatus(int id)
    {
        // Arrange
        var model = new UpdateAnswerDto()
        {
            Id = id,
            Content = "Test Content"
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client_Admin.PutAsync("/Answers/" + id + 1, httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task PutAnswer_WithValidModelThatDoesntExist_ReturnsNotFoundStatus(int id)
    {
        // Arrange
        var model = new UpdateAnswerDto()
        {
            Id = id,
            Content = "Test Content"
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client_Admin.PutAsync("/Answers/" + id, httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    public async Task PutQuestion_WithValidModelThatHasAnotherOwner_ReturnsForbiddenStatus(int id)
    {
        // Arrange
        var model = new UpdateAnswerDto()
        {
            Id = id,
            Content = "Test Content"
        };

        var httpContent = model.ToJsonHttpContent();

        // Act
        var response = await _client_User.PutAsync("/Answers/" + id, httpContent);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }
    #endregion

    #region Delete
    [Theory]
    [InlineData(23)]
    [InlineData(24)]
    public async Task DeleteAnswer_ForAdmin_ReturnsNoContentStatus(int id)
    {
        // Act
        var response = await _client_Admin.DeleteAsync("/Answers/" + id);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DeleteAnswer_ForQuestionThatDoesntExist_ReturnsNotFoundStatus(int id)
    {
        // Act
        var response = await _client_User.DeleteAsync("/Answers/" + id);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    public async Task DeleteAnswer_ForQuestionWithAnotherOwner_ReturnsForbiddenStatus(int id)
    {
        // Act
        var response = await _client_User.DeleteAsync("/Answers/" + id);

        // Assert 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }
    #endregion
}
