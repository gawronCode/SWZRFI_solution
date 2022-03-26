using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Interfaces;

namespace SWZRFI.DAL.Repositories.Implementations
{
    public class AnswerRepo : IAnswerRepo
    {

        private readonly ApplicationContext _context;

        public AnswerRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Answer entity)
        {
            await _context.Answers.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Answer entity)
        {
            _context.Answers.Remove(entity);
            return await Save();
        }

        public Task<bool> Exists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Answer>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Answer>> GetAnswersByQuestionId(int id)
        {
            var answers = await _context.Answers.Where(q => q.QuestionId == id).ToListAsync();
            return answers;
        }

        public Task<Answer> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public Task<bool> Update(Answer entity)
        {
            throw new NotImplementedException();
        }
    }
}
