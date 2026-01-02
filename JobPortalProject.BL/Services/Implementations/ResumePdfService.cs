using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories;
using JobPortalProject.DA.Repositories.Contracts;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

//public class ResumePdfService : IResumePdfService
//{
//    private readonly IResumeService _resumeService;
//    private readonly IHttpClientFactory _httpClientFactory;

//    public ResumePdfService(IResumeService resumeService, IHttpClientFactory httpClientFactory)
//    {
//        _resumeService = resumeService;
//        _httpClientFactory = httpClientFactory;
//        QuestPDF.Settings.License = LicenseType.Community;
//    }

//    public async Task<byte[]> GenerateResumePdfAsync(int resumeId, int languageId)
//    {
//        var resume = await _resumeService.GetResumeWithDetailsAsync(resumeId, languageId);

//        if (resume == null)
//            throw new InvalidOperationException("Resume not found");

//        byte[] profileImageBytes = null;
//        if (!string.IsNullOrEmpty(resume.PersonalInfo?.ImageUrl))
//        {
//            profileImageBytes = await DownloadImageAsync(resume.PersonalInfo.ImageUrl);
//        }

//        return await Task.Run(() => GeneratePdf(resume, languageId, profileImageBytes));
//    }

//    private async Task<byte[]> DownloadImageAsync(string imageUrl)
//    {
//        try
//        {
//            var httpClient = _httpClientFactory.CreateClient();
//            httpClient.Timeout = TimeSpan.FromSeconds(10);
//            return await httpClient.GetByteArrayAsync(imageUrl);
//        }
//        catch (Exception)
//        {
//            return null;
//        }
//    }

//    private byte[] GeneratePdf(Resume resume, int languageId, byte[] profileImageBytes)
//    {
//        var brandColor = "#00A7AC";
//        var lightGray = "#F5F5F5";

//        var document = Document.Create(container =>
//        {
//            container.Page(page =>
//            {
//                page.Size(PageSizes.A4);
//                page.Margin(0);
//                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

//                page.Content().Column(column =>
//                {
//                    // Top brand color bar
//                    column.Item().Height(3).Background(brandColor);

//                    // Header section with profile and contact
//                    column.Item().Background(Colors.White).PaddingVertical(30).PaddingHorizontal(40).Row(row =>
//                    {
//                        // Left side: Profile image and name
//                        row.RelativeItem().Row(leftRow =>
//                        {
//                            // Profile image
//                            if (profileImageBytes != null && profileImageBytes.Length > 0)
//                            {
//                                leftRow.ConstantItem(120).Height(120).BorderColor(brandColor).Border(3)
//                                    .Padding(3).Image(profileImageBytes, ImageScaling.FitArea);
//                            }

//                            // Name
//                            leftRow.RelativeItem().PaddingLeft(20).AlignMiddle().Column(nameCol =>
//                            {
//                                var personalInfoTranslation = resume.PersonalInfo?.Translations
//                                    .FirstOrDefault(t => t.LanguageId == languageId);

//                                if (personalInfoTranslation != null)
//                                {
//                                    nameCol.Item().Text($"{personalInfoTranslation.FirstName} {personalInfoTranslation.LastName}")
//                                        .FontSize(26).Bold().FontColor(brandColor);
//                                }
//                            });
//                        });

//                        // Right side: Contact info box
//                        row.ConstantItem(280).AlignRight().Column(contactCol =>
//                        {
//                            contactCol.Item().Text("Contact Info")
//                                .FontSize(16).Bold().FontColor(Colors.Black);

//                            contactCol.Item().PaddingTop(12).Column(infoCol =>
//                            {
//                                if (resume.PersonalInfo != null)
//                                {
//                                    infoCol.Item().PaddingBottom(6).Row(r =>
//                                    {
//                                        r.RelativeItem().AlignRight().Text($"Phone: {resume.PersonalInfo.PhoneNumber}")
//                                            .FontSize(11).FontColor(Colors.Grey.Darken2);
//                                    });

//                                    infoCol.Item().PaddingBottom(6).Row(r =>
//                                    {
//                                        r.RelativeItem().AlignRight().Text($"Email: {resume.PersonalInfo.WorkEmail}")
//                                            .FontSize(11).FontColor(Colors.Grey.Darken2);
//                                    });
//                                }
//                            });
//                        });
//                    });

//                    // Content sections
//                    column.Item().PaddingHorizontal(40).Column(contentColumn =>
//                    {
//                        var translation = resume.Translations.FirstOrDefault(t => t.LanguageId == languageId);

//                        // Career Objective
//                        if (translation != null && !string.IsNullOrEmpty(translation.About))
//                        {
//                            contentColumn.Item().PaddingTop(20).Column(col =>
//                            {
//                                col.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten1)
//                                    .PaddingBottom(8).Text("Career Objective")
//                                    .FontSize(14).Bold().FontColor(Colors.Black);

//                                col.Item().PaddingTop(10).Text(translation.About)
//                                    .FontSize(10).FontColor(Colors.Grey.Darken2).LineHeight(1.4f);
//                            });
//                        }

//                        // Personal Information
//                        contentColumn.Item().PaddingTop(20).Column(col =>
//                        {
//                            col.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten1)
//                                .PaddingBottom(8).Text("Personal Information")
//                                .FontSize(14).Bold().FontColor(Colors.Black);

//                            col.Item().PaddingTop(10).Column(infoCol =>
//                            {
//                                if (resume.PersonalInfo != null)
//                                {
//                                    infoCol.Item().PaddingBottom(5).Row(r =>
//                                    {
//                                        r.ConstantItem(140).Text("Date of Birth:").Bold().FontSize(10);
//                                        r.RelativeItem().Text($"{resume.PersonalInfo.BirthDate:dd-MMM-yy}")
//                                            .FontSize(10).FontColor(Colors.Grey.Darken2);
//                                    });

//                                    if (resume.PersonalInfo.Address != null)
//                                    {
//                                        var address = BuildAddressString(resume.PersonalInfo.Address, languageId);
//                                        if (!string.IsNullOrEmpty(address))
//                                        {
//                                            infoCol.Item().PaddingBottom(5).Row(r =>
//                                            {
//                                                r.ConstantItem(140).Text("Permanent Address:").Bold().FontSize(10);
//                                                r.RelativeItem().Text(address).FontSize(10).FontColor(Colors.Grey.Darken2);
//                                            });
//                                        }
//                                    }

//                                    var personalInfoTranslation = resume.PersonalInfo?.Translations
//                                        .FirstOrDefault(t => t.LanguageId == languageId);

//                                    if (personalInfoTranslation != null)
//                                    {
//                                        infoCol.Item().PaddingBottom(5).Row(r =>
//                                        {
//                                            r.ConstantItem(140).Text("Gender:").Bold().FontSize(10);
//                                            r.RelativeItem().Text(resume.PersonalInfo.Gender.ToString() ?? "")
//                                                .FontSize(10).FontColor(Colors.Grey.Darken2);
//                                        });
//                                    }
//                                }
//                            });
//                        });

//                        // Educational Qualification
//                        if (resume.Educations?.Any() == true)
//                        {
//                            contentColumn.Item().PaddingTop(20).Column(col =>
//                            {
//                                col.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten1)
//                                    .PaddingBottom(8).Text("Educational Qualification")
//                                    .FontSize(14).Bold().FontColor(Colors.Black);

//                                col.Item().PaddingTop(10).Column(eduCol =>
//                                {
//                                    int index = 1;
//                                    foreach (var education in resume.Educations.OrderByDescending(e => e.StartDate))
//                                    {
//                                        var eduTranslation = education.Translations
//                                            .FirstOrDefault(t => t.LanguageId == languageId);

//                                        if (eduTranslation != null)
//                                        {
//                                            eduCol.Item().PaddingBottom(15).Row(eduRow =>
//                                            {
//                                                // Number and date
//                                                eduRow.ConstantItem(180).Column(leftCol =>
//                                                {
//                                                    leftCol.Item().Row(r =>
//                                                    {
//                                                        r.AutoItem().Width(20).Text($"{index}.")
//                                                            .Bold().FontColor(brandColor).FontSize(11);
//                                                        r.RelativeItem().Text($"{education.StartDate:MMM yyyy} - {education.EndDate:MMM yyyy}")
//                                                            .FontSize(10).FontColor(Colors.Grey.Darken1);
//                                                    });
//                                                });

//                                                // Details
//                                                eduRow.RelativeItem().PaddingLeft(10).Column(detailCol =>
//                                                {
//                                                    detailCol.Item().Text(eduTranslation.SchoolName)
//                                                        .FontSize(12).Bold().FontColor(Colors.Black);

//                                                    detailCol.Item().PaddingTop(5).Row(r =>
//                                                    {
//                                                        r.AutoItem().Text("Education Level: ").Bold().FontSize(10);
//                                                        r.AutoItem().Text(education.EducationType)
//                                                            .FontSize(10).FontColor(Colors.Grey.Darken2);
//                                                    });

//                                                    detailCol.Item().PaddingTop(3).Row(r =>
//                                                    {
//                                                        r.AutoItem().Text("My Major: ").Bold().FontSize(10);
//                                                        r.AutoItem().Text(eduTranslation.MajorName)
//                                                            .FontSize(10).FontColor(Colors.Grey.Darken2);
//                                                    });
//                                                });
//                                            });

//                                            index++;
//                                        }
//                                    }
//                                });
//                            });
//                        }

//                        // Professionals Information
//                        if (resume.Experiences?.Any() == true)
//                        {
//                            contentColumn.Item().PaddingTop(20).Column(col =>
//                            {
//                                col.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten1)
//                                    .PaddingBottom(8).Text("Professionals Information")
//                                    .FontSize(14).Bold().FontColor(Colors.Black);

//                                col.Item().PaddingTop(10).Column(expCol =>
//                                {
//                                    int index = 1;
//                                    foreach (var experience in resume.Experiences.OrderByDescending(e => e.StartDate))
//                                    {
//                                        var expTranslation = experience.Translations
//                                            .FirstOrDefault(t => t.LanguageId == languageId);

//                                        if (expTranslation != null)
//                                        {
//                                            expCol.Item().PaddingBottom(15).Row(expRow =>
//                                            {
//                                                // Number and date
//                                                expRow.ConstantItem(180).Column(leftCol =>
//                                                {
//                                                    leftCol.Item().Row(r =>
//                                                    {
//                                                        r.AutoItem().Width(20).Text($"{index}.")
//                                                            .Bold().FontColor(brandColor).FontSize(11);
//                                                        r.RelativeItem().Text($"{experience.StartDate:MMM yyyy} - {experience.EndDate:MMM yyyy}")
//                                                            .FontSize(10).FontColor(Colors.Grey.Darken1);
//                                                    });
//                                                });

//                                                // Details
//                                                expRow.RelativeItem().PaddingLeft(10).Column(detailCol =>
//                                                {
//                                                    detailCol.Item().Text(expTranslation.CompanyName)
//                                                        .FontSize(12).Bold().FontColor(Colors.Black);

//                                                    detailCol.Item().PaddingTop(5).Row(r =>
//                                                    {
//                                                        r.AutoItem().Text("Position: ").Bold().FontSize(10);
//                                                        r.AutoItem().Text(expTranslation.Position)
//                                                            .FontSize(10).FontColor(Colors.Grey.Darken2);
//                                                    });

//                                                    detailCol.Item().PaddingTop(3).Row(r =>
//                                                    {
//                                                        r.AutoItem().Text("Responsibility: ").Bold().FontSize(10);
//                                                        r.RelativeItem().Text(expTranslation.Responsibility)
//                                                            .FontSize(10).FontColor(Colors.Grey.Darken2);
//                                                    });
//                                                });
//                                            });

//                                            index++;
//                                        }
//                                    }
//                                });
//                            });
//                        }

//                        // Declaration
//                        contentColumn.Item().PaddingTop(25).Column(col =>
//                        {
//                            col.Item().Text("Declaration & Authentication-")
//                                .FontSize(12).Bold().FontColor(Colors.Black);

//                            col.Item().PaddingTop(8).Text("I do hereby declare that the information given above is true of my knowledge.")
//                                .FontSize(10).FontColor(Colors.Grey.Darken2);

//                            col.Item().PaddingTop(15).Column(signCol =>
//                            {
//                                signCol.Item().Text("Yours Faithful,")
//                                    .FontSize(10).FontColor(Colors.Grey.Darken2);

//                                var personalInfoTranslation = resume.PersonalInfo?.Translations
//                                    .FirstOrDefault(t => t.LanguageId == languageId);

//                                if (personalInfoTranslation != null)
//                                {
//                                    signCol.Item().PaddingTop(5).Text($"{personalInfoTranslation.FirstName} {personalInfoTranslation.LastName}")
//                                        .FontSize(11).Bold().FontColor(Colors.Black);
//                                }
//                            });
//                        });

//                        // Bottom spacing
//                        contentColumn.Item().PaddingBottom(30);
//                    });
//                });
//            });
//        });

//        return document.GeneratePdf();
//    }


//    private string BuildAddressString(Address address, int languageId)
//    {
//        var parts = new List<string>();

//        var cityTranslation = address.City?.CityTranslations
//            ?.FirstOrDefault(t => t.LanguageId == languageId);

//        if (cityTranslation != null && !string.IsNullOrEmpty(cityTranslation.Name))
//        {
//            parts.Add(cityTranslation.Name);
//        }

//        var countryTranslation = address.City?.Country?.Translations
//            ?.FirstOrDefault(t => t.LanguageId == languageId);

//        if (countryTranslation != null && !string.IsNullOrEmpty(countryTranslation.Name))
//        {
//            parts.Add(countryTranslation.Name);
//        }

//        var addressTranslation = address.AddressTranslations
//            ?.FirstOrDefault(t => t.LanguageId == languageId);

//        if (addressTranslation != null && !string.IsNullOrEmpty(addressTranslation.Street))
//        {
//            parts.Add(addressTranslation.Street);
//        }

//        return string.Join(", ", parts);
//    }
//}


public class ResumePdfService : IResumePdfService
{
    private readonly IResumeService _resumeService;
    private readonly IHttpClientFactory _httpClientFactory;

    // Define colors
    private static readonly string BrandColor = "#00A7AC";
    private static readonly string TextColor = "#2b2b2b";
    private static readonly string LightText = "#666666";

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

        byte[] profileImageBytes = null;
        if (!string.IsNullOrEmpty(resume.PersonalInfo?.ImageUrl))
        {
            profileImageBytes = await DownloadImageAsync(resume.PersonalInfo.ImageUrl);
        }

        return GeneratePdf(resume, languageId, profileImageBytes);
    }

    private async Task<byte[]> DownloadImageAsync(string imageUrl)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            return await httpClient.GetByteArrayAsync(imageUrl);
        }
        catch
        {
            return null;
        }
    }

    private byte[] GeneratePdf(Resume resume, int languageId, byte[] profileImageBytes)
    {
        var translation = resume.Translations.FirstOrDefault(t => t.LanguageId == languageId);
        var personalInfoTrans = resume.PersonalInfo?.Translations.FirstOrDefault(t => t.LanguageId == languageId);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial).FontColor(TextColor));

                page.Content().Column(col =>
                {
                    // 1. Header Area
                    col.Item().Row(row =>
                    {
                        // Left: Author Area
                        row.RelativeItem().Column(authorCol =>
                        {
                            // Image Logic
                            if (profileImageBytes != null)
                            {
                                authorCol.Item().Width(100).Height(100)
                                    .CornerRadius(50)
                                    .Image(profileImageBytes, ImageScaling.FitArea);
                            }
                            else
                            {
                                authorCol.Item().Width(100).Height(100)
                                    .Background(Colors.Grey.Lighten3)
                                    .CornerRadius(50);
                            }

                            // Name
                            authorCol.Item().PaddingTop(15).Text($"{personalInfoTrans?.FirstName} {personalInfoTrans?.LastName}")
                                .FontSize(20).Bold().FontColor(BrandColor);
                        });

                        // Right: Contact Area
                        row.RelativeItem().AlignRight().Column(contactCol =>
                        {
                            contactCol.Item().Text("Contact Info").FontSize(16).Bold().FontColor(Colors.Black);

                            contactCol.Item().Height(10);

                            contactCol.Item().Column(list =>
                            {
                                if (!string.IsNullOrEmpty(resume.PersonalInfo?.PhoneNumber))
                                {
                                    list.Item().PaddingBottom(5).Text(t =>
                                    {
                                        t.Span("Phone: ").Bold();
                                        t.Span(resume.PersonalInfo.PhoneNumber).FontColor(LightText);
                                    });
                                }

                                if (!string.IsNullOrEmpty(resume.PersonalInfo?.WorkEmail))
                                {
                                    list.Item().PaddingBottom(5).Text(t =>
                                    {
                                        t.Span("Email: ").Bold();
                                        t.Span(resume.PersonalInfo.WorkEmail).FontColor(LightText);
                                    });
                                }
                            });
                        });
                    });

                    col.Item().Height(30);

                    // 2. Career Objective
                    if (translation != null && !string.IsNullOrEmpty(translation.About))
                    {
                        col.Item().Element(c => SectionTitle(c, "Career Objective"));
                        col.Item().Text(translation.About).LineHeight(1.5f).FontColor(LightText);
                        col.Item().Height(20);
                    }

                    // 3. Personal Information
                    col.Item().Element(c => SectionTitle(c, "Personal Information"));
                    col.Item().Column(infoCol =>
                    {
                        infoCol.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(130);
                                columns.RelativeColumn();
                            });

                            table.Cell().Element(LabelStyle).Text("Date of Birth:");
                            table.Cell().Element(ValueStyle).Text(resume.PersonalInfo?.BirthDate.ToString("dd-MMM-yyyy"));

                            var address = BuildAddressString(resume.PersonalInfo?.Address, languageId);
                            table.Cell().Element(LabelStyle).Text("Permanent Address:");
                            table.Cell().Element(ValueStyle).Text(address);

                            table.Cell().Element(LabelStyle).Text("Gender:");
                            table.Cell().Element(ValueStyle).Text(resume.PersonalInfo?.Gender.ToString() ?? "-");
                        });
                    });
                    col.Item().Height(20);

                    // 4. Educational Qualification
                    if (resume.Educations?.Any() == true)
                    {
                        col.Item().Element(c => SectionTitle(c, "Educational Qualification"));

                        int index = 1;
                        foreach (var edu in resume.Educations.OrderByDescending(e => e.StartDate))
                        {
                            var eduTrans = edu.Translations.FirstOrDefault(t => t.LanguageId == languageId);
                            if (eduTrans == null) continue;

                            col.Item().PaddingBottom(15).Row(row =>
                            {
                                row.ConstantItem(160).Text(t =>
                                {
                                    t.Span($"{index}. ").Bold().FontColor(BrandColor);
                                    t.Span($"{edu.StartDate:MMM yyyy} - {edu.EndDate:MMM yyyy}");
                                });

                                row.RelativeItem().Column(details =>
                                {
                                    details.Item().Text(eduTrans.SchoolName).Bold().FontSize(11);

                                    details.Item().PaddingTop(2).Text(t =>
                                    {
                                        t.Span("Education Level: ").Bold();
                                        t.Span(edu.EducationType.ToString()).FontColor(LightText);
                                    });

                                    details.Item().PaddingTop(2).Text(t =>
                                    {
                                        t.Span("My Major: ").Bold();
                                        t.Span(eduTrans.MajorName).FontColor(LightText);
                                    });
                                });
                            });
                            index++;
                        }
                        col.Item().Height(10);
                    }

                    // 5. Professionals Information
                    if (resume.Experiences?.Any() == true)
                    {
                        col.Item().Element(c => SectionTitle(c, "Professionals Information"));

                        int index = 1;
                        foreach (var exp in resume.Experiences.OrderByDescending(e => e.StartDate))
                        {
                            var expTrans = exp.Translations.FirstOrDefault(t => t.LanguageId == languageId);
                            if (expTrans == null) continue;

                            col.Item().PaddingBottom(15).Row(row =>
                            {
                                row.ConstantItem(160).Text(t =>
                                {
                                    t.Span($"{index}. ").Bold().FontColor(BrandColor);
                                    t.Span($"{exp.StartDate:MMM yyyy} - {exp.EndDate:MMM yyyy}");
                                });

                                row.RelativeItem().Column(details =>
                                {
                                    details.Item().Text(expTrans.CompanyName).Bold().FontSize(11);

                                    details.Item().PaddingTop(2).Text(t =>
                                    {
                                        t.Span("Position: ").Bold();
                                        t.Span(expTrans.Position).FontColor(LightText);
                                    });

                                    if (!string.IsNullOrEmpty(expTrans.Responsibility))
                                    {
                                        details.Item().PaddingTop(2).Text(t =>
                                        {
                                            t.Span("Responsibility: ").Bold();
                                            t.Span(expTrans.Responsibility).FontColor(LightText);
                                        });
                                    }
                                });
                            });
                            index++;
                        }
                    }

                    // 6. Declaration
                    col.Item().PaddingTop(20).Column(dec =>
                    {
                        dec.Item().Text("Declaration & Authentication-").Bold().FontSize(12);
                        dec.Item().Text("I do hereby declare that the information given above is true of my knowledge.")
                            .FontColor(LightText).Italic();
                    });

                    // 7. Signature
                    col.Item().PaddingTop(30).Column(sign =>
                    {
                        sign.Item().Text("Yours Faithful,").FontColor(LightText);
                        sign.Item().PaddingTop(5).Text($"{personalInfoTrans?.FirstName} {personalInfoTrans?.LastName}")
                            .Bold().FontSize(12);
                    });
                });
            });
        });

        return document.GeneratePdf();
    }

    // --- Helpers ---

    // FIX: Removed .Dotted() and used LineHorizontal for the dash effect
    private static void SectionTitle(IContainer container, string title)
    {
        container.PaddingBottom(15).Row(row =>
        {
            // 1. Text on the Left
            row.AutoItem().Text(title).FontSize(14).Bold().FontColor(Colors.Black);

            // 2. Line filling the remaining space on the Right
            // We use LineHorizontal(1) which creates a standard solid line.
            // Note: QuestPDF borders/lines are solid by default. 
            row.RelativeItem().PaddingLeft(10).AlignMiddle().LineHorizontal(1).LineColor(BrandColor);
        });
    }

    private static IContainer LabelStyle(IContainer container)
    {
        return container.PaddingBottom(6).PaddingRight(10);
    }

    private static IContainer ValueStyle(IContainer container)
    {
        return container.PaddingBottom(6).AlignLeft();
    }

    private string BuildAddressString(Address address, int languageId)
    {
        if (address == null) return string.Empty;

        var parts = new List<string>();

        var cityTrans = address.City?.CityTranslations?.FirstOrDefault(t => t.LanguageId == languageId);
        if (!string.IsNullOrEmpty(cityTrans?.Name)) parts.Add(cityTrans.Name);

        var countryTrans = address.City?.Country?.Translations?.FirstOrDefault(t => t.LanguageId == languageId);
        if (!string.IsNullOrEmpty(countryTrans?.Name)) parts.Add(countryTrans.Name);

        var addrTrans = address.AddressTranslations?.FirstOrDefault(t => t.LanguageId == languageId);
        if (!string.IsNullOrEmpty(addrTrans?.Street)) parts.Add(addrTrans.Street);

        return string.Join(", ", parts);
    }
}