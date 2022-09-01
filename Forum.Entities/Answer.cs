using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Entities;

public class Answer
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public int QuestionId { get; set; }
    public virtual Question Question { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; }

    public Guid? AuthorId { get; set; }
    public virtual User Author { get; set; }

    [NotMapped]
    public int RatingValue
    {
        get
        {
            if (Ratings is null || Ratings.Count == 0)
            {
                return 0;
            }

            return Ratings.Select(r => r.Value).Sum();
        }
    }
}
