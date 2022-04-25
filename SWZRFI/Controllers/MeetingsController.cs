using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Contexts;
using SWZRFI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.Controllers
{
    public class MeetingsController : BaseController
    {
        private readonly ApplicationContext _context;

        public MeetingsController(ApplicationContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var currentUser = await _context.UserAccount.FirstOrDefaultAsync(q => q.Email == GetCurrentUserEmail());

            var meetings = await _context
                .JobOfferUserMeetings
                .AsSplitQuery()
                .Include(q => q.JobOffer)
                .Include(q => q.Application)
                .ThenInclude(q => q.UserAccount)
                .Where(q => q.UserAccountId == currentUser.Id).ToListAsync();


            var recruitmentMeetings = meetings.Select(q => new RecruitmentMeeting
            {
                JobOffer = q.JobOffer,
                JobOfferApplication = q,
                Candidate = q.UserAccount,
                Recruiter = currentUser
            }).OrderBy(q => q.JobOfferApplication.MeetingDateTime)
            .ToList();


            return View(recruitmentMeetings);
        }
    }
}
