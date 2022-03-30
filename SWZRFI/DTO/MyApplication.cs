using SWZRFI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DTO
{
    public class MyApplication
    {
        public int Id { get; set; }
        public int JobOfferId { get; set; }
        public string JobOfferName { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public string CompanyName { get; set; }
        public QuestionnaireAccess QuestionnaireAccess { get; set; }
    }
}
