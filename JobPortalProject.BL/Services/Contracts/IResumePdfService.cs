using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IResumePdfService
    {
        Task<byte[]> GenerateResumePdfAsync(int resumeId, int languageId);
    }
}
