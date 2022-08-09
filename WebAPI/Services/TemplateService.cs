using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using WebAPI.Services.Models;

namespace WebAPI.Services
{
    public class TemplateService
    {
        
        private readonly IConfiguration _configuration;

        public TemplateService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConfirmationMail(TemplateParameter code)
        {
            return Get("confirmationMail.html", new List<TemplateParameter> {code});
        }

        public string GetConfirmationMailSubject()
        {
            return Get("confirmationMail_Subject.txt");
        }
        
        public string Get(string file, List<TemplateParameter> parameters = null)
        {
            var strings = File.ReadAllLines($"{Root}/{file}");

            if (parameters != null)
            {
                for (int i = 0; i < strings.Length; i++)
                {
                    foreach (var p in parameters)
                    {
                        strings[i] = strings[i].Replace(p.FormattedVariable, p.Value, StringComparison.Ordinal);
                    }
                }
            }

            return string.Join("", strings);
        }

        private string Root => _configuration["TemplatesRoot"];

    }
}