using AutoMapper;
using Forum.Entities;
using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Forum.WebAPI.Services;

public interface IUserService
{
    ClaimsPrincipal User { get; }
    Guid? UserId { get; }
    Task<UserDto> RegisterUserAsync(RegisterUserDto registerUserDto);
    Task<string> LoginUserAsync(LoginUserDto loginUserDto);
    bool VerifyUserData(LoginUserDto loginUserDto);
}

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserService(IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal User => httpContextAccessor.HttpContext?.User;

    public Guid? UserId => User is null ? null : Guid.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
       
    public Task<string> LoginUserAsync(LoginUserDto loginUserDto)
    {
        User user = userRepository.GetUser(u => u.Username == loginUserDto.Username);

        return Task.FromResult(userRepository.CreateToken(user));
    }

    public async Task<UserDto> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        CreatePasswordHash(registerUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        try
        {
            User user = mapper.Map<User>(registerUserDto);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await userRepository.InsertUserAsync(user);
            await userRepository.SaveAsync();

            return mapper.Map<UserDto>(userRepository.GetUser(u => u.Username == user.Username));
        }
        catch(Exception ex)
        {
            var sqlException = ex.InnerException as SqlException;

            if (sqlException.Number == 2601 || sqlException.Number == 2627)
            {
                throw new DuplicateNameException("User with given Email or Username already exists.");
            }
            else
            {
                throw new Exception(ex.Message);
            }
        }
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    public bool VerifyUserData(LoginUserDto loginUserDto)
    {
        User user = userRepository.GetUser(u => u.Username == loginUserDto.Username);

        if (user == null) return false;

        using (var hmac = new HMACSHA512(user.PasswordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginUserDto.Password));
            return computedHash.SequenceEqual(user.PasswordHash);
        }
    }
}
