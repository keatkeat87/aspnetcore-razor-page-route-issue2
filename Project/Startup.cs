using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Project
{
    public class AmpRouteConstraint : IRouteConstraint
    {
        public static readonly string ConstraintSymbol = "amp";

        public bool Match(HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {

            //validate input params  
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));
            if (route == null) throw new ArgumentNullException(nameof(route));
            if (routeKey == null) throw new ArgumentNullException(nameof(routeKey));
            if (values == null) throw new ArgumentNullException(nameof(values));

            if (values.TryGetValue(routeKey, out var routeValue))
            {
                string amp = routeValue.ToString().ToLower();
                return amp == ConstraintSymbol;
            }

            return false;
        }
    }

    public class LanguageRouteConstraint : IRouteConstraint
    {
        public static readonly string ConstraintSymbol = "language";

        public bool Match(HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            //validate input params  
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));
            if (route == null) throw new ArgumentNullException(nameof(route));
            if (routeKey == null) throw new ArgumentNullException(nameof(routeKey));
            if (values == null) throw new ArgumentNullException(nameof(values));

            if (values.TryGetValue(routeKey, out var routeValue))
            {
                string language = routeValue.ToString();
                return language == "en-US" || language == "zh-Hans";
            }

            return false;
        }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RouteOptions>(routeOptions =>
            {
                routeOptions.ConstraintMap.Add(AmpRouteConstraint.ConstraintSymbol, typeof(AmpRouteConstraint));
                routeOptions.ConstraintMap.Add(LanguageRouteConstraint.ConstraintSymbol, typeof(LanguageRouteConstraint));
            });

            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Home/Index", "/{amp:amp}/{language:language}");  // amp + specify language
                options.Conventions.AddPageRoute("/Home/Index", "/{amp:amp}");                      // amp + default language
                options.Conventions.AddPageRoute("/Home/Index", "/{language:language}");            // no amp + specify language
                options.Conventions.AddPageRoute("/Home/Index", "");                                // no amp + default language
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
