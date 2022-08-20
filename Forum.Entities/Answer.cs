namespace Forum.Entities;

public class Answer
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime Date { get; set; }
    public int QuestionId { get; set; }
    public Question Question { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; }
}
