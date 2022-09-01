namespace Forum.WebAPI.Dto_s;

public class UpdateAnswerDto
{
    public int Id { get; set; }

    public string Content { get; set; }

    public int QuestionId { get; set; }

    public Guid? AuthorId { get; set; }
}
