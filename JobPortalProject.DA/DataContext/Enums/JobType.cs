using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.DA.DataContext.Enums
{
    public enum JobType
    {
        FullTime,
        PartTime,
        Temporary,
        Freelance,
        Remote,
        Internship
    }

    public enum EducationType
    {
        Associate,
        Bachelor,
        Master,
        Doctoral
    }

    public enum SalaryTypeDuration
    {
        PerMonth,
        PerYear,
        PerHour,
        ProjectBased
    }

    public enum Gender
    {
        Male,
        Female,
        Both
    }

}
