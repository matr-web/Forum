namespace Forum.WebAPI.Dto_s;

public class QuestionDto
{
    public string Topic { get; set; }

    public string Content { get; set; }

    public DateTime Date { get; set; }

    public Guid? AuthorId { get; set; }

    public virtual string AuthorFullName { get; set; }

    public virtual ICollection<AnswerDto> Answers { get; set; }

    public int RatingValue { get; set; }
}
