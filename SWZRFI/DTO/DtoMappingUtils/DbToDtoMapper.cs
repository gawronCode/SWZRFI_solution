using SWZRFI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DTO.DtoMappingUtils
{
    public static class DbToDtoMapper
    {
        public static JobOfferDetails MapToDto(this JobOffer jobOffer)
        {
            return new JobOfferDetails
            {
                JobOfferId = jobOffer.Id,
                Title = jobOffer.Title,
                SkillLevel = GetSkillLevel(jobOffer.SkillLevel),
                Description = GetSplittedEnumerable(jobOffer.Description),
                Requirements = GetSplittedEnumerable(jobOffer.Requirements),
                OptionalRequirements = GetSplittedEnumerable(jobOffer.OptionalRequirements),
                AdditionalBenefits = GetSplittedEnumerable(jobOffer.AdditionalBenefits),
                LowerBoundSallary = jobOffer.LowerBoundSallary,
                UpperBoundSallary = jobOffer.UpperBoundSallary,
                Country = GetStringOrNull(jobOffer.Country),
                City = GetStringOrNull(jobOffer.City),
                Street = GetStringOrNull(jobOffer.Street),
                HouseNumber = GetStringOrNull(jobOffer.HouseNumber),
                CompanyName = jobOffer.Company.Name,
                CompanyDescription = GetSplittedEnumerable(jobOffer.Company.Description),
                CompanyCity = GetStringOrNull(jobOffer.Company.City),
                CompanyStreet = GetStringOrNull(jobOffer.Company.Street),
                CompanyCountry = GetStringOrNull(jobOffer.Company.Country),
                CompanyHouseNumber = GetStringOrNull(jobOffer.Company.HouseNumber)
            };
        }

        private static string GetStringOrNull(string source)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrEmpty(source))
                return null;

            return source.Trim();
        }

        private static IEnumerable<string> GetSplittedEnumerable(string source)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source))
                yield return "";
            else
                foreach (var item in source.Split('\n'))
                    yield return item.Trim();
        }

        private static string GetSkillLevel(int skillLevel)
        {
            switch (skillLevel)
            {
                case 0:
                    return "Junior";
                case 1:
                    return "Mid";
                case 2:
                    return "Senior";
                default:
                    return "Error";
            }
        }
    }
}
