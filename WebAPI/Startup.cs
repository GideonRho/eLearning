using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAPI.Contexts;
using WebAPI.Services;

namespace WebAPI
{
    public class Startup
    {

        public static IHostApplicationLifetime applicationLifetime;
        
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        private readonly IWebHostEnvironment _environment;
        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(
                opt 
                    => opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), 
                        provider => provider.EnableRetryOnFailure())
                    );
            services.AddControllers();
            services.AddScoped<IDataService, DataService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IImportService, ImportService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<QuestionnaireService>();
            services.AddScoped<StatisticService>();
            services.AddScoped<ProductService>();
            services.AddScoped<MailService>();
            services.AddScoped<VerificationService>();
            services.AddScoped<TemplateService>();
            services.AddAuthentication("AuthSecurityScheme")
                .AddCookie("AuthSecurityScheme", options =>
                {
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Events.OnRedirectToLogin = (context) =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments($@"{System.AppDomain.CurrentDomain.BaseDirectory}/WebAPI.xml");
            });
            var origins = new List<string>();
            Configuration.GetSection("Origins").Bind(origins);
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder
                            .WithOrigins(origins.ToArray())
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            applicationLifetime = lifetime;
            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Configuration["SwaggerFile"], "My API V1");
            });
 
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }
    }
}