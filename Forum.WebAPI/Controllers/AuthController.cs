using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService userService;

    public AuthController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserDto>> RegisterAsync(RegisterUserDto registerUserDto)
    {
        UserDto userDto = await userService.RegisterUserAsync(registerUserDto);

        return Ok(userDto);
    }


    [HttpPost("Login")]
    public async Task<ActionResult<string>> LoginAsync(LoginUserDto loginUserDto)
    {
        if (await userService.VerifyUserData(loginUserDto) is false)
        {
            return BadRequest("Wrong Username or Password.");
        }

        var token = await userService.LoginUserAsync(loginUserDto);

        return Ok(token);
    }
}
