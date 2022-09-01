namespace Forum.WebAPI.Dto_s;

public class CreateAnswerDto
{
    public string Content { get; set; }

    public int QuestionId { get; set; }

    public Guid? AuthorId { get; set; }
}
