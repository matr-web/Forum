namespace Forum.Entities;

public class User 
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Question> Questions { get; set; }
    public ICollection<Answer> Answers { get; set; }
}
