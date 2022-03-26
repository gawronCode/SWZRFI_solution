﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Interfaces;

namespace SWZRFI.DAL.Repositories.Implementations
{
    public class QuestionnaireRepo : IQuestionnaireRepo
    {

        private readonly ApplicationContext _context;

        public QuestionnaireRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Questionnaire entity)
        {
            await _context.Questionnaires.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Questionnaire entity)
        {
            _context.Questionnaires.Remove(entity);
            return await Save();
        }

        public Task<bool> Exists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Questionnaire>> GetAll()
        {
            var questionnaires = await _context.Questionnaires.ToListAsync();
            return questionnaires;
        }

        public async Task<Questionnaire> GetById(int id)
        {
            var questionnaire = await _context.Questionnaires.FirstOrDefaultAsync(q => q.Id == id);
            return questionnaire;
        }

        public async Task<bool> Save()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Questionnaire entity)
        {
            _context.Update(entity);
            return await Save();
        }
    }
}