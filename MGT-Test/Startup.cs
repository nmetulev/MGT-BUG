using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MGT_Test
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
     services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(o => {
                    o.UsePkce = true;
                    o.ClientId = "ab58afcf-ef01-4cc0-89fd-3cabceab38c3";
                    o.TenantId = "801e6804-9cb1-4277-be16-491ffcc0070a";
                    o.Domain = "https://localhost:5001";
                    o.Instance = "https://login.microsoftonline.com";
                    o.CallbackPath = "/signin-oidc";
                    o.ResponseType = "code";
                    var defaultBackChannel = new HttpClient();
                    defaultBackChannel.DefaultRequestHeaders.Add("Origin", "thisismyapp");
                    o.Backchannel = defaultBackChannel;
                });
      services.AddAuthorization(options =>
      {
              // By default, all incoming requests will be authorized according to the default policy
              options.FallbackPolicy = options.DefaultPolicy;
      });
      services.AddRazorPages()
          .AddMvcOptions(options => { })
          .AddMicrosoftIdentityUI();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapRazorPages();
        endpoints.MapControllers();
      });
    }
  }
}
