using System;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace MekashronTestTask
{
    public class LoginController : SurfaceController
    {
        public LoginController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {

        }

        [HttpGet]
        public IActionResult CallWebService([FromQuery] string login, string password)
        {
            ClientClass client = new ClientClass("http://isapi.icu-tech.com/icutech-test.dll/soap/IICUTech");

            string result = client.CallWebServiceMethodAsync("Login", login, password).Result;

            return Content(result);
        }
    }
}

