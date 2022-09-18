namespace Forum.WebAPI.Dto_s;

public class AnswerDto
{
    public string Content { get; set; }

    public DateTime Date { get; set; }

    public int QuestionId { get; set; }

    public Guid? AuthorId { get; set; }

    public string AuthorFullName { get; set; }

    public int RatingValue { get; set; }
}
