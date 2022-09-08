using AutoMapper;
using Forum.Entities;
using Forum.WebAPI.Dto_s;
using Forum.WebAPI.Repositories;
using System.Security.Cryptography;

namespace Forum.WebAPI.Services;

public interface IUserService
{
    Task<UserDto> RegisterUserAsync(RegisterUserDto registerUserDto);
    Task<string> LoginUserAsync(LoginUserDto loginUserDto);
    Task<bool> VerifyUserData(LoginUserDto loginUserDto);
}

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    public async Task<string> LoginUserAsync(LoginUserDto loginUserDto)
    {
        User user = await userRepository.GetUserByNameAsync(loginUserDto.Username);

        return userRepository.CreateToken(user);
    }

    public async Task<UserDto> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        CreatePasswordHash(registerUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        User user = mapper.Map<User>(registerUserDto);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await userRepository.InsertUserAsync(user);
        await userRepository.SaveAsync();

        return mapper.Map<UserDto>(await userRepository.GetUserByNameAsync(user.Username));
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    public async Task<bool> VerifyUserData(LoginUserDto loginUserDto)
    {
        User user = await userRepository.GetUserByNameAsync(loginUserDto.Username);

        if (user == null) return false;

        using (var hmac = new HMACSHA512(user.PasswordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginUserDto.Password));
            return computedHash.SequenceEqual(user.PasswordHash);
        }
    }
}
