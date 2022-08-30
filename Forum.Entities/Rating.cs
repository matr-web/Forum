namespace Forum.Entities;

public class Rating
{
    public int Id { get; set; }

    public int Value { get; set; }

    public Guid AuthorId { get; set; }
    public virtual User Author { get; set; }

    public int? QuestionId { get; set; }
    public virtual Question Question { get; set; }

    public int? AnswerId { get; set; } 
    public virtual Answer Answer { get; set; }
}
