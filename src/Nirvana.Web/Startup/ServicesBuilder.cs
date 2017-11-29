using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;

namespace Nirvana.Web.Startup
{
    public class ServicesBuilder
    {

       public  ServicesBuilder AddMvc(IServiceCollection services)
        {
            
            services.AddMvc();
            return this;
        }
       
       public  ServicesBuilder AddCors(IServiceCollection services)
        {
            services.AddCors();
            return this;
        }

        public ServicesBuilder ConfigurePublicCors(IApplicationBuilder app) 
        {
            
            app.UseCors(options=>options.AllowAnyOrigin());
            return this;
        }



        public ServicesBuilder AddPolocyRequirement(IServiceCollection services,string name, IAuthorizationRequirement requirement)
        {
            services.AddAuthorization(options => options.AddPolicy(name,policy => policy.Requirements.Add(requirement)));
            return this;
        }

    }
}
