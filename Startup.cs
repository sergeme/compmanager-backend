using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using CompManager.Helpers;
using CompManager.Middleware;
using CompManager.Services;
using CompManager.Entities;

namespace CompManager
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    // add services to the DI container
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<DataContext>();
      services.AddCors();
      services.AddControllers().AddNewtonsoftJson(options =>
      {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

      });
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      services.AddSwaggerGen(options =>
      {
        options.CustomSchemaIds(type => type.ToString());
      });

      // configure strongly typed settings object
      services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

      // configure DI for application services
      services.AddScoped<IAccountService, AccountService>();
      services.AddScoped<IClassService, ClassService>();
      services.AddScoped<ITagService, TagService>();
      services.AddScoped<ICommentService, CommentService>();
      services.AddScoped<IReviewService, ReviewService>();
      services.AddScoped<ICompetenceService, CompetenceService>();
      services.AddScoped<ICourseService, CourseService>();
      services.AddScoped<ICurriculumService, CurriculumService>();
      services.AddScoped<IDepartmentService, DepartmentService>();
      services.AddScoped<IEmailService, EmailService>();
      services.AddScoped<ILocationService, LocationService>();
      services.AddScoped<IProcessService, ProcessService>();
      services.AddScoped<IProcessTypeService, ProcessTypeService>();
    }

    // configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, DataContext context)
    {
      // migrate database changes on startup (includes initial db creation)
      context.Database.Migrate();

      app.UsePathBase("/api/v1");
      // generated swagger json and swagger ui middleware
      app.UseSwagger();
      app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Compmanager Backend"));

      app.UseRouting();

      // global cors policy
      app.UseCors(x => x
          .SetIsOriginAllowed(origin => true)
          .WithOrigins(Configuration.GetSection("Appsettings")["FrontendBaseUrl"])
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());

      // global error handler
      app.UseMiddleware<ErrorHandlerMiddleware>();

      // custom jwt auth middleware
      app.UseMiddleware<JwtMiddleware>();

      app.UseEndpoints(x => x.MapControllers());
    }
  }
}
