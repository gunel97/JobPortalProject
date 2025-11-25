using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.DA.Repositories
{
    public class LanguageRepository:EfCoreRepositoryAsync<Language>, ILanguageRepository
    {
        public LanguageRepository(AppDbContext context) : base(context) { }
    }
}
