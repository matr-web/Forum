using FluentAssertions;
using Forum.WebAPI.Dto_s;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Forum.Tests;

/// <summary>
/// Tests for Auth Controller. 
/// Tests work fine with current local db state.
/// </summary>
public class AuthControllerTests
{
    private HttpClient _client;

    public AuthControllerTests()
    {
        _client = new WebApplicationFactory<Program>().CreateClient();
    }

    #region Register
    [Fact]
    public async Task RegisterUser_ForValidModel_ReturnsOk()
    {
        // Arrange
        var registerUser = new RegisterUserDto()
        {
            Username = "testsUsername",
            FirstName= "Test",
            LastName= "Test",
            Email = "test@test.com",
            Password = "password123",
            ConfirmPassword = "password123",
            RoleId= 1,
        };

        var httpContent = registerUser.ToJsonHttpContent();

        // Act
        var response = await _client.PostAsync("Auth/Register", httpContent);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task RegisterUser_ForDifferentPasswords_ReturnsBadRequest()
    {
        // Arrange
        var registerUser = new RegisterUserDto()
        {
            Username = "testsUsername",
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "password133",
            ConfirmPassword = "password123",
            RoleId = 1,
        };

        var httpContent = registerUser.ToJsonHttpContent();

        // Act
        var response = await _client.PostAsync("Auth/Register", httpContent);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
    #endregion

    #region Login
    [Fact]
    public async Task Login_ForRegisteredUser_ReturnsOk()
    {
        // Arrange
        var loginDto = new LoginUserDto()
        {
            Username = "testsUsername",
            Password = "password123"
        };

        var httpContent = loginDto.ToJsonHttpContent();

        // Act
        var response = await _client.PostAsync("Auth/Login", httpContent);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_ForNotRegisteredUser_ReturnsBadRequest()
    {
        // Arrange
        var loginDto = new LoginUserDto()
        {
            Username = "testsUsername1",
            Password = "password123"
        };

        var httpContent = loginDto.ToJsonHttpContent();

        // Act
        var response = await _client.PostAsync("Auth/Login", httpContent);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
    #endregion
}