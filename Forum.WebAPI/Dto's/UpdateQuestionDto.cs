namespace Forum.WebAPI.Dto_s;

public class UpdateQuestionDto
{
    public int Id { get; set; }

    public string Topic { get; set; }

    public string Content { get; set; }

    public Guid? AuthorId { get; set; }
}
