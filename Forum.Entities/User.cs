using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Entities;

public class User 
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public string Password { get; set; }

    public int RoleId { get; set; }
    public virtual Role Role { get; set; }

    public virtual ICollection<Question> Questions { get; set; }
    public virtual ICollection<Answer> Answers { get; set; }
    public virtual ICollection<Rating> Ratings { get; set; }

    [NotMapped]
    public string ConfirmPassword { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}
