using Forum.Entities;

namespace Forum.WebAPI.Authorization;

public class Resource
{
    public Resource(Question question)
    {
        this.question = question;
    }

    public Resource(Answer answer)
    {
        this.answer = answer;
    }

    public Question question { get; set; }

    public Answer answer { get; set; }
}
