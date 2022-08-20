namespace Forum.Entities;

public class Question
{
    public int Id { get; set; }
    public string Topic { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime Date { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; }
    public ICollection<Answer> Answers { get; set; }
}
