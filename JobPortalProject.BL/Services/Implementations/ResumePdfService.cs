using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories;
using JobPortalProject.DA.Repositories.Contracts;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class ResumePdfService : IResumePdfService
{
    private readonly IResumeService _resumeService;
    private readonly IHttpClientFactory _httpClientFactory;

    public ResumePdfService(IResumeService resumeService, IHttpClientFactory httpClientFactory)
    {
        _resumeService = resumeService;
        _httpClientFactory = httpClientFactory;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> GenerateResumePdfAsync(int resumeId, int languageId)
    {
        var resume = await _resumeService.GetResumeWithDetailsAsync(resumeId, languageId);

        if (resume == null)
            throw new InvalidOperationException("Resume not found");

        // Download profile image if exists
        byte[] profileImageBytes = null;
        if (!string.IsNullOrEmpty(resume.PersonalInfo?.ImageUrl))
        {
            profileImageBytes = await DownloadImageAsync(resume.PersonalInfo.ImageUrl);
        }

        // PDF generation is CPU-bound, so we run it on a separate thread
        return await Task.Run(() => GeneratePdf(resume, languageId, profileImageBytes));
    }

    private async Task<byte[]> DownloadImageAsync(string imageUrl)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            return await httpClient.GetByteArrayAsync(imageUrl);
        }
        catch (Exception)
        {
            // If image download fails, return null and continue without image
            return null;
        }
    }

    private byte[] GeneratePdf(Resume resume, int languageId, byte[] profileImageBytes)
    {
        // Define custom brand color
        var brandColor = "#00A7AC";

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                page.Header().Element(ComposeHeader);
                page.Content().Element(content => ComposeContent(content, resume, languageId, profileImageBytes, brandColor));
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" of ");
                    x.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().PaddingBottom(10).Border(1).BorderColor(Colors.Grey.Lighten2);
            });
        });
    }

    private void ComposeContent(IContainer container, Resume resume, int languageId, byte[] profileImageBytes, string brandColor)
    {
        var translation = resume.Translations.FirstOrDefault(t => t.LanguageId == languageId);
        var personalInfoTranslation = resume.PersonalInfo?.Translations
            .FirstOrDefault(t => t.LanguageId == languageId);

        container.Column(column =>
        {
            // Personal Information Section
            column.Item().PaddingBottom(15).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    if (personalInfoTranslation != null)
                    {
                        col.Item().Text($"{personalInfoTranslation.FirstName} {personalInfoTranslation.LastName}")
                            .FontSize(24).Bold().FontColor(brandColor);
                    }

                    if (resume.PersonalInfo != null)
                    {
                        col.Item().PaddingTop(5).Text(resume.PersonalInfo.WorkEmail).FontSize(10);
                        col.Item().Text(resume.PersonalInfo.PhoneNumber).FontSize(10);
                        col.Item().Text($"Birth Date: {resume.PersonalInfo.BirthDate:dd/MM/yyyy}").FontSize(10);

                        if (resume.PersonalInfo.Address != null)
                        {
                            var address = BuildAddressString(resume.PersonalInfo.Address, languageId);
                            if (!string.IsNullOrEmpty(address))
                            {
                                col.Item().Text(address).FontSize(10);
                            }
                        }
                    }
                });

                // Profile image
                if (profileImageBytes != null && profileImageBytes.Length > 0)
                {
                    row.ConstantItem(100).Height(100).Border(2).BorderColor(brandColor)
                        .Padding(2).Image(profileImageBytes, ImageScaling.FitArea);
                }
            });

            column.Item().PaddingBottom(5).LineHorizontal(2).LineColor(brandColor);

            // About Section
            if (translation != null && !string.IsNullOrEmpty(translation.About))
            {
                column.Item().PaddingTop(15).Column(col =>
                {
                    col.Item().Text("About").FontSize(16).Bold().FontColor(brandColor);
                    col.Item().PaddingTop(5).Text(translation.About).FontSize(10).Justify();
                });
            }

            // Skills Section
            if (translation?.Skills?.Any() == true)
            {
                column.Item().PaddingTop(15).Column(col =>
                {
                    col.Item().Text("Skills").FontSize(16).Bold().FontColor(brandColor);
                    col.Item().PaddingTop(5).Text(string.Join(" • ", translation.Skills)).FontSize(10);
                });
            }

            // Languages Section
            if (translation?.Languages?.Any() == true)
            {
                column.Item().PaddingTop(15).Column(col =>
                {
                    col.Item().Text("Languages").FontSize(16).Bold().FontColor(brandColor);
                    col.Item().PaddingTop(5).Text(string.Join(" • ", translation.Languages)).FontSize(10);
                });
            }

            // Experience Section
            if (resume.Experiences?.Any() == true)
            {
                column.Item().PaddingTop(15).Column(col =>
                {
                    col.Item().Text("Experience").FontSize(16).Bold().FontColor(brandColor);

                    foreach (var experience in resume.Experiences.OrderByDescending(e => e.StartDate))
                    {
                        var expTranslation = experience.Translations
                            .FirstOrDefault(t => t.LanguageId == languageId);

                        if (expTranslation != null)
                        {
                            col.Item().PaddingTop(10).Column(expCol =>
                            {
                                expCol.Item().Row(expRow =>
                                {
                                    expRow.RelativeItem().Text(expTranslation.Position).Bold().FontSize(12);
                                    expRow.AutoItem().Text($"{experience.StartDate:MMM yyyy} - {experience.EndDate:MMM yyyy}")
                                        .FontSize(10).FontColor(Colors.Grey.Darken1);
                                });

                                expCol.Item().Text(expTranslation.CompanyName).Italic().FontSize(11);
                                expCol.Item().PaddingTop(3).Text(expTranslation.Responsibility).FontSize(10);
                            });
                        }
                    }
                });
            }

            // Education Section
            if (resume.Educations?.Any() == true)
            {
                column.Item().PaddingTop(15).Column(col =>
                {
                    col.Item().Text("Education").FontSize(16).Bold().FontColor(brandColor);

                    foreach (var education in resume.Educations.OrderByDescending(e => e.StartDate))
                    {
                        var eduTranslation = education.Translations
                            .FirstOrDefault(t => t.LanguageId == languageId);

                        if (eduTranslation != null)
                        {
                            col.Item().PaddingTop(10).Column(eduCol =>
                            {
                                eduCol.Item().Row(eduRow =>
                                {
                                    eduRow.RelativeItem().Text(eduTranslation.MajorName).Bold().FontSize(12);
                                    eduRow.AutoItem().Text($"{education.StartDate:MMM yyyy} - {education.EndDate:MMM yyyy}")
                                        .FontSize(10).FontColor(Colors.Grey.Darken1);
                                });

                                eduCol.Item().Text(eduTranslation.SchoolName).Italic().FontSize(11);
                                eduCol.Item().Text($"Type: {education.EducationType}").FontSize(10);
                            });
                        }
                    }
                });
            }
        });
    }

    private string BuildAddressString(Address address, int languageId)
    {
        var parts = new List<string>();

        // Get street from AddressTranslation
        var addressTranslation = address.AddressTranslations
            ?.FirstOrDefault(t => t.LanguageId == languageId);

        if (addressTranslation != null && !string.IsNullOrEmpty(addressTranslation.Street))
        {
            parts.Add(addressTranslation.Street);
        }

        // Get city name from CityTranslation
        var cityTranslation = address.City?.CityTranslations
            ?.FirstOrDefault(t => t.LanguageId == languageId);

        if (cityTranslation != null && !string.IsNullOrEmpty(cityTranslation.Name))
        {
            parts.Add(cityTranslation.Name);
        }

        // Get country name from CountryTranslation
        var countryTranslation = address.City?.Country?.Translations
            ?.FirstOrDefault(t => t.LanguageId == languageId);

        if (countryTranslation != null && !string.IsNullOrEmpty(countryTranslation.Name))
        {
            parts.Add(countryTranslation.Name);
        }

        return string.Join(", ", parts);
    }
}