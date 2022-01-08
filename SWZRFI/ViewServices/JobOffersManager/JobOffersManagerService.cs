using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Interfaces;



namespace SWZRFI.ViewServices.JobOffersManager
{
    public class JobOffersManagerService : IJobOffersManagerService
    {
        private readonly IJobOfferRepo _jobOfferRepo;
        private readonly IUserRepo _userRepo;


        public JobOffersManagerService(IJobOfferRepo jobOfferRepo,
                                       IUserRepo userRepo)
        {
            _jobOfferRepo = jobOfferRepo;
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<JobOffer>> GetIndexPageData(string email)
        {
            var user = await GetUser(email);
            return user.Company.JobOffers;
        }

        public async Task CreateJobOffer(string email, JobOffer jobOffer)
        {
            var user = await GetUser(email);
            jobOffer.CreationDate = DateTime.Now;
            jobOffer.LastModified = jobOffer.CreationDate;
            jobOffer.CreatorUserAccountId = user.Id;
            jobOffer.EditorUserAccountId = user.Id;
            jobOffer.CompanyId = user.CompanyId ?? throw new Exception();

            await _jobOfferRepo.CreateJobOfferAsync(jobOffer);
        }

        public async Task<JobOffer> GetJobOfferForEdition(int jobOfferId)
        {
            return await _jobOfferRepo.GetJobOfferByIdAsync(jobOfferId);
        }

        public async Task SaveEditedJobOffer(string email, JobOffer jobOffer)
        {
            var user = await GetUser(email);
            jobOffer.LastModified = DateTime.Now;
            jobOffer.EditorUserAccountId = user.Id;
            await _jobOfferRepo.UpdateJobOfferAsync(jobOffer);
        }

        public async Task<bool> RemoveJobOffer(string email, int jobOfferId)
        {
            var user = await GetUser(email);
            var company = user.Company;

            var jobOffer = company?.JobOffers?.FirstOrDefault(q => q.Id == jobOfferId);

            if (jobOffer == null)
                return false;

            await _jobOfferRepo.RemoveJobOfferAsync(jobOffer);
            return true;
        }


        private async Task<UserAccount> GetUser(string email)
        {
            return await _userRepo.GetUserByEmailAsync(email);
        }



    }
}
