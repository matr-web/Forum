using Forum.Entities;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace Forum.WebAPI.Repositories;

public interface IUserRepository
{
    User GetUser(Expression<Func<User, bool>> predicate);
    Task InsertUserAsync(User user);
    Task DeleteUserAsync(Guid userId);
    string CreateToken(User user);
    Task SaveAsync();
}

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext context;
    private readonly IConfiguration configuration;

    public UserRepository(DatabaseContext context, IConfiguration configuration)
    {
        this.context = context;
        this.configuration = configuration;
    }

    public User GetUser(Expression<Func<User, bool>> predicate)
    {
        return context.Users
            .Include(u => u.Role)
            .Where(predicate)
            .FirstOrDefault();
    }

    public async Task InsertUserAsync(User user)
    {
        await context.Users.AddAsync(user);
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        User user = await context.Users.FindAsync(userId);
        await Task.FromResult(context.Users.Remove(user));
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    string IUserRepository.CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            configuration.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
