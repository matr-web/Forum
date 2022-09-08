using Bogus;
using Forum.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.WebAPI;

public class SeedData
{
    public static void GenerateUsers(DatabaseContext context)
    {
        if (!context.Roles.Any())
        {
            Role administrator = new Role()
            {
                Id = 1,
                Name = "Administrator"
            };

            Role user = new Role()
            {
                Id = 2,
                Name = "User"
            };

            context.Database.OpenConnection();

            try
            {
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Roles ON");

                context.Roles.AddRange(administrator, user);

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Roles OFF");
            }
            finally
            {
                context.Database.CloseConnection();
            }     
        }

        if (!context.Users.Any())
        { 
            Randomizer.Seed = new Random(1);

            var administatorRole = context.Roles.FirstOrDefault(r => r.Name == "Administrator");
            var userRole = context.Roles.FirstOrDefault(r => r.Name == "User");

            var administratorsGenerator = new Faker<User>()
               .RuleFor(u => u.FirstName, f => f.Person.FirstName)
               .RuleFor(u => u.LastName, f => f.Person.LastName)
               .RuleFor(u => u.Email, f => f.Person.Email)
               .RuleFor(u => u.Username, f => f.Person.UserName)
               .RuleFor(u => u.Password, f => f.Person.FirstName + "123")
               .RuleFor(u => u.RoleId, 1)
               .RuleFor(u => u.Role, administatorRole);

            var usersGenerator = new Faker<User>()
               .RuleFor(u => u.FirstName, f => f.Person.FirstName)
               .RuleFor(u => u.LastName, f => f.Person.LastName)
               .RuleFor(u => u.Email, f => f.Person.Email)
               .RuleFor(u => u.Username, f => f.Person.UserName)
               .RuleFor(u => u.RoleId, 2)
               .RuleFor(u => u.Role, userRole); 

            var administrators = administratorsGenerator.Generate(20);
            var users = usersGenerator.Generate(20);

            context.AddRange(administrators);
            context.AddRange(users);

            context.SaveChanges();
        }
    }

    public static void GenerateQuestions(DatabaseContext context)
    {
        if (!context.Questions.Any())
        {
            Randomizer.Seed = new Random(1);

            var users = context.Users.ToList();

            var questionsGenerator = new Faker<Question>()
                .RuleFor(q => q.Topic, f => f.Lorem.Sentence(6, 8))
                .RuleFor(q => q.Content, f => f.Lorem.Sentences(4))
                .RuleFor(q => q.Date, f => f.Date.Past())
                .RuleFor(q => q.AuthorId, f => f.PickRandom(users).Id);

            var questions = questionsGenerator.Generate(40);

            context.AddRange(questions);
            context.SaveChanges();
        }
    }

    public static void GenerateAnswers(DatabaseContext context)
    {
        if (!context.Answers.Any())
        {
            Randomizer.Seed = new Random(1);

            var users = context.Users.ToList();
            var questions = context.Questions.ToList();

            var answerGenerator = new Faker<Answer>()
                .RuleFor(a => a.Content, f => f.Lorem.Sentences(4))
                .RuleFor(a => a.Date, f => f.Date.Past())
                .RuleFor(a => a.AuthorId, f => f.PickRandom(users).Id)
                .RuleFor(a => a.QuestionId, f => f.PickRandom(questions).Id);

            var answers = answerGenerator.Generate(80);

            context.AddRange(answers);
            context.SaveChanges();
        }
    }

    public static void GenerateRatings(DatabaseContext context)
    {
        if (!context.Ratings.Any())
        {
            Randomizer.Seed = new Random(1);

            var users = context.Users.ToList();
            var questions = context.Questions.ToList();
            var answers = context.Answers.ToList();
            var ratingValues = new List<int>(){ -1, 1 };

            var questionsRatingsGenerator = new Faker<Rating>()
                .RuleFor(a => a.Value, f => f.PickRandom(ratingValues))
                .RuleFor(a => a.AuthorId, f => f.PickRandom(users).Id)
                .RuleFor(a => a.QuestionId, f => f.PickRandom(questions).Id);

            var answersRatingsGenerator = new Faker<Rating>()
                .RuleFor(a => a.Value, f => f.PickRandom(ratingValues))
                .RuleFor(a => a.AuthorId, f => f.PickRandom(users).Id)
                .RuleFor(a => a.AnswerId, f => f.PickRandom(answers).Id);

            var questionRatings = questionsRatingsGenerator.Generate(80);
            var answerRatings = answersRatingsGenerator.Generate(80);

            context.AddRange(questionRatings);
            context.AddRange(answerRatings);
            context.SaveChanges();
        }
    }
}
