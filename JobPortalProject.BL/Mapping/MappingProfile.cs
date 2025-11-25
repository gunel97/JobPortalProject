using AutoMapper;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.CountryViewModels;
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
                .MapFrom(src => src.JobCategoryTranslations!.FirstOrDefault() == null ? "" :
                src.JobCategoryTranslations!.FirstOrDefault()!.Name))
                .ReverseMap();
            CreateMap<JobCategory, JobCategoryCreateViewModel>().ReverseMap();
            CreateMap<JobCategory, JobCategoryUpdateViewModel>().ReverseMap();

            CreateMap<JobCategoryTranslation, JobCategoryTranslationViewModel>().ReverseMap();
            CreateMap<JobCategoryTranslation, JobCategoryTranslationCreateViewModel>().ReverseMap();
            CreateMap<JobCategoryTranslation, JobCategoryTranslationUpdateViewModel>().ReverseMap();

            CreateMap<Country, CountryViewModel>()
                .ForMember(x=>x.Name, opt=>opt.MapFrom(src=>src.Translations!.FirstOrDefault()==null ? "" :
                src.Translations!.FirstOrDefault()!.Name))
                .ReverseMap();
            CreateMap<Country, CountryCreateViewModel>().ReverseMap();
            CreateMap<Country, CountryUpdateViewModel>().ReverseMap();

            CreateMap<CountryTranslation, CountryTranslationViewModel>().ReverseMap();
            CreateMap<CountryTranslation, CountryTranslationCreateViewModel>().ReverseMap();
            CreateMap<CountryTranslation, CountryTranslationUpdateViewModel>().ReverseMap();

            CreateMap<City, CityViewModel>()
                .ForMember(x=>x.Name, opt => opt
                .MapFrom(src=>src.CityTranslations!.FirstOrDefault()==null ? "" : 
                src.CityTranslations!.FirstOrDefault()!.Name))
                //.ForMember(x=>x.CountryName, opt=>opt.MapFrom(src=>src.Country == null ? "" :
                //src.Country.Translations.FirstOrDefault() ==null ? "" : src.Country.Translations.FirstOrDefault()!.Name))
                .ReverseMap();
            CreateMap<City, CityCreateViewModel>().ReverseMap();
            CreateMap<City, CityUpdateViewModel>().ReverseMap();

            CreateMap<CityTranslation, CityTranslationViewModel>().ReverseMap();
            CreateMap<CityTranslation, CityTranslationCreateViewModel>().ReverseMap();
            CreateMap<CityTranslation, CityTranslationUpdateViewModel>().ReverseMap();

            CreateMap<Address, AddressViewModel>()
                .ForMember(x=>x.Street, opt=>opt
                .MapFrom(src=>src.AddressTranslations!.FirstOrDefault()==null ? "" :
                src.AddressTranslations!.FirstOrDefault()!.Street))
                .ReverseMap();
            CreateMap<Address, AddressCreateViewModel>().ReverseMap();
            CreateMap<Address, AddressUpdateViewModel>().ReverseMap();

            CreateMap<AddressTranslation, AddressTranslationViewModel>().ReverseMap();
            CreateMap<AddressTranslation, AddressTranslationCreateViewModel>().ReverseMap();
            CreateMap<AddressTranslation, AddressTranslationUpdateViewModel>().ReverseMap();



        }
    }
}
