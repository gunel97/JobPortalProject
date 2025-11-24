using AutoMapper;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.DA.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Language, LanguageViewModel>().ReverseMap();
            CreateMap<Language, LanguageCreateViewModel>().ReverseMap();
            CreateMap<Language, LanguageUpdateViewModel>().ReverseMap();

            CreateMap<JobCategory, JobCategoryViewModel>()
                .ForMember(dest => dest.Name, opt => opt
                .MapFrom(src => src.JobCategoryTranslations!.FirstOrDefault() == null ? "" : src.JobCategoryTranslations!.FirstOrDefault()!.Name))
                .ReverseMap();
            CreateMap<JobCategory, JobCategoryCreateViewModel>().ReverseMap();
            CreateMap<JobCategory, JobCategoryUpdateViewModel>().ReverseMap();

            CreateMap<JobCategoryTranslation, JobCategoryTranslationViewModel>().ReverseMap();
            CreateMap<JobCategoryTranslation, JobCategoryTranslationCreateViewModel>().ReverseMap();
            CreateMap<JobCategoryTranslation, JobCategoryTranslationUpdateViewModel>().ReverseMap();



        }
    }
}
