using Forum.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.WebAPI.Repositories
{
    public interface IQuestionRepository : IDisposable
    {
        IEnumerable<Question> GetQuestions();
        Question GetQuestionByID(int questionId);
        void InsertQuestion(Question question);
        void DeleteQuestion(int questionId);
        void UpdateQuestion(Question question);
        void Save();
    }

    public class QuestionRepository : IQuestionRepository, IDisposable
    {
        private DatabaseContext context;

        public QuestionRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public IEnumerable<Question> GetQuestions()
        {
            return context.Questions.ToList();
        }

        public Question GetQuestionByID(int id)
        {
            return context.Questions.Find(id);
        }

        public void InsertQuestion(Question question)
        {
            context.Questions.Add(question);
        }

        public void DeleteQuestion(int questionID)
        {
            Question question = context.Questions.Find(questionID);
            context.Questions.Remove(question);
        }

        public void UpdateQuestion(Question question)
        {
            context.Entry(question).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
