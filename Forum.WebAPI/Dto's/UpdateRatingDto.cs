namespace Forum.WebAPI.Dto_s
{
    public class UpdateRatingDto
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public Guid AuthorId { get; set; }

        public int? QuestionId { get; set; }

        public int? AnswerId { get; set; }
    }
}
