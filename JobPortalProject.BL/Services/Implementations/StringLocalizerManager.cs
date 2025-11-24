using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Implementations
{
    public class StringLocalizerManager
    {
        private readonly IStringLocalizer _stringLocalizer;

        public StringLocalizerManager(IStringLocalizerFactory stringLocalizerFactory)
        {
            _stringLocalizer = stringLocalizerFactory.Create("SharedResources", "JobPortalProject.UserMvc");
        }

        public string GetValue(string key)
        {
            var test =  _stringLocalizer.GetString(key);
            return test;
        }
    }
}
