using AutoMapper;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.CompanySocialViewModels;
using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.SocialMediaViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
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
                .ForMember(x=>x.CountryName, opt=>opt
                .MapFrom(src=>src.Country==null ? "" :
                src.Country.Translations.FirstOrDefault()!.Name))
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
                .ForMember(x=>x.CompanyIds, opt=>opt
                .MapFrom(src=>src.CompanyAddresses.Select(at=>at.CompanyId).ToList()))
                .ReverseMap();
            CreateMap<Address, AddressCreateViewModel>().ReverseMap();
            CreateMap<Address, AddressUpdateViewModel>().ReverseMap();

            CreateMap<AddressTranslation, AddressTranslationViewModel>().ReverseMap();
            CreateMap<AddressTranslation, AddressTranslationCreateViewModel>().ReverseMap();
            CreateMap<AddressTranslation, AddressTranslationUpdateViewModel>().ReverseMap();

            CreateMap<Company, CompanyViewModel>()
                .ForMember(x => x.Name, opt => opt
                .MapFrom(src => src.CompanyTranslations!.FirstOrDefault() == null ? "" :
                src.CompanyTranslations!.FirstOrDefault()!.Name))
                .ForMember(src => src.MainAddress, opt => opt
                .MapFrom(src => src.CompanyAddresses == null ? null :
                src.CompanyAddresses.FirstOrDefault(a => a.IsMain)!.Address))
                .ForMember(x => x.CompanyAddresses, opt => opt.MapFrom(src => src.CompanyAddresses
                .Select(a => a.Address != null ? a.Address : null).ToList()))
                .ForMember(x=>x.CategoryName, opt => opt
                .MapFrom(src=>src.CompanyType == null ? "" :
                src.CompanyType.CompanyTypeTranslations.FirstOrDefault()!.Name))
                .ReverseMap();
            CreateMap<Company, CompanyCreateViewModel>().ReverseMap();
            CreateMap<Company, CompanyUpdateViewModel>().ReverseMap();

            CreateMap<CompanyTranslation, CompanyTranslationViewModel>().ReverseMap();
            CreateMap<CompanyTranslation, CompanyTranslationCreateViewModel>().ReverseMap();
            CreateMap<CompanyTranslation, CompanyTranslationUpdateViewModel>().ReverseMap();

            CreateMap<WorkingFieldTranslation, WorkingFieldTranslationViewModel>().ReverseMap();
            CreateMap<WorkingFieldTranslation, WorkingFieldTranslationCreateViewModel>().ReverseMap();
            CreateMap<WorkingFieldTranslation, WorkingFieldTranslationUpdateViewModel>().ReverseMap();

            CreateMap<WorkingField, WorkingFieldViewModel>()
                .ForMember(x => x.Name, opt => opt
                .MapFrom(src => src.Translations.FirstOrDefault()==null ? "" :
                src.Translations!.FirstOrDefault()!.Name))
                .ForMember(x=>x.Description, opt=> opt
                .MapFrom(src => src.Translations.FirstOrDefault() == null ? "" :
                src.Translations!.FirstOrDefault()!.Description))
                .ReverseMap();
            CreateMap<WorkingField, WorkingFieldCreateViewModel>().ReverseMap();
            CreateMap<WorkingField, WorkingFieldUpdateViewModel>().ReverseMap();

            CreateMap<SocialMedia, SocialMediaViewModel>().ReverseMap();
            CreateMap<SocialMedia, SocialMediaCreateViewModel>().ReverseMap();
            CreateMap<SocialMedia, SocialMediaUpdateViewModel>().ReverseMap();

            CreateMap<CompanySocial, CompanySocialViewModel>().ReverseMap();
            CreateMap<CompanySocial, CompanySocialCreateViewModel>().ReverseMap();
            CreateMap<CompanySocial, CompanySocialUpdateViewModel>().ReverseMap();

            CreateMap<CompanyType, CompanyTypeViewModel>()
                .ForMember(x=>x.Name, opt => opt
                .MapFrom(src=>src.CompanyTypeTranslations.FirstOrDefault()==null ? "" :
                src.CompanyTypeTranslations!.FirstOrDefault()!.Name))
                .ReverseMap();
            CreateMap<CompanyType, CompanyTypeCreateViewModel>().ReverseMap();
            CreateMap<CompanyType, CompanyTypeUpdateViewModel>().ReverseMap();

            CreateMap<CompanyTypeTranslation, CompanyTypeTranslationViewModel>().ReverseMap();
            CreateMap<CompanyTypeTranslation, CompanyTypeTranslationCreateViewModel>().ReverseMap();
            CreateMap<CompanyTypeTranslation, CompanyTypeTranslationUpdateViewModel>().ReverseMap();


        }
    }
}
