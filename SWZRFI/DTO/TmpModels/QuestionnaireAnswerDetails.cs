﻿using System;

namespace SWZRFI.DTO.TmpModels
{
    public class QuestionnaireAnswerDetails
    {
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public DateTime? AnswerDate { get; set; }
        public int AnswerCount { get; set; }
        public int AnswerSum { get; set; }

        public double GetAverageScore()
        {
            return (double) AnswerSum / (double) AnswerCount;
        }
    }
}
