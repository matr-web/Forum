namespace Forum.WebAPI.Dto_s;

public class CreateRatingDto
{
    public int Value { get; set; }

    public Guid AuthorId { get; set; }

    public int? QuestionId { get; set; }

    public int? AnswerId { get; set; }
}
