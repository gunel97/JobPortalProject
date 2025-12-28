using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.DA.DataContext.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Implementations
{
    public class EnumManager:IEnumService
    {
        private readonly StringLocalizerManager _localizer;

        public EnumManager(StringLocalizerManager localizer)
        {
            _localizer = localizer;
        }

        public List<SelectListItem> GetJobTypeListItems()
        {
            var jobTypeListItems = new List<SelectListItem>();
            var jobTypes = Enum.GetValues(typeof(JobType)).Cast<JobType>().ToList();
            jobTypes.ForEach(x => jobTypeListItems.Add(
                new SelectListItem(_localizer.GetValue(x.ToString()), ((int)x).ToString())));
            return jobTypeListItems;
        }

        public List<SelectListItem> GetGenderListItems()
        {
            var genderListItems = new List<SelectListItem>();
            var genders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            genders.ForEach(x => genderListItems.Add(
                new SelectListItem(_localizer.GetValue(x.ToString()), ((int)x).ToString())));

            return genderListItems;
        }

        public List<SelectListItem> GetSalaryTypeListItems()
        {
            var salaryTypeListItems = new List<SelectListItem>();
            var salaryTypes = Enum.GetValues(typeof(SalaryTypeDuration)).Cast<SalaryTypeDuration>().ToList();
            salaryTypes.ForEach(x => salaryTypeListItems.Add(
                new SelectListItem(_localizer.GetValue(x.ToString()), ((int)x).ToString())));
            return salaryTypeListItems;
        }

        public List<SelectListItem> GetEducationTypeListItems()
        {
            var educationTypeListItems = new List<SelectListItem>();
            var educationTypes = Enum.GetValues(typeof(EducationType)).Cast<EducationType>().ToList();
            educationTypes.ForEach(x => educationTypeListItems.Add(
                new SelectListItem(_localizer.GetValue(x.ToString()), ((int)x).ToString())));
            return educationTypeListItems;
        }

      

        public List<SelectListItem> GetJobApplicationStatusListItems()
        {
            var jobApplicationStatusItems =new List<SelectListItem>();
            var jobApplicationStatuses = Enum.GetValues(typeof(JobApplicationStatus)).Cast<JobApplicationStatus>().ToList();
            jobApplicationStatuses.ForEach(x => jobApplicationStatusItems.Add(
            new SelectListItem(_localizer.GetValue(x.ToString()), ((int)x).ToString())));

            return jobApplicationStatusItems;
        }
    }
}
