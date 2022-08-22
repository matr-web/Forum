using Forum.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.WebAPI.Repositories
{

    public interface IAnswerRepository : IDisposable
    {
        IEnumerable<Answer> GetAnswers();
        Answer GetAnswerByID(int answerId);
        void InsertAnswer(Answer answer);
        void DeleteAnswer(int answerId);
        void UpdateAnswer(Answer answer);
        void Save();
    }

    public class AnswerRepository : IAnswerRepository, IDisposable
    {
        private DatabaseContext context;

        public AnswerRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public IEnumerable<Answer> GetAnswers()
        {
            return context.Answers.ToList();
        }

        public Answer GetAnswerByID(int id)
        {
            return context.Answers.Find(id);
        }

        public void InsertAnswer(Answer answer)
        {
            context.Answers.Add(answer);
        }

        public void DeleteAnswer(int answerID)
        {
            Answer answer = context.Answers.Find(answerID);
            context.Answers.Remove(answer);
        }

        public void UpdateAnswer(Answer answer)
        {
            context.Entry(answer).State = EntityState.Modified;
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
