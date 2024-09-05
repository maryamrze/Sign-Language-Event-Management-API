using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using a2.Data;
using a2.Handler;
using a2.Helper;

namespace a2{
    public class Program{
        public static void Main(string[] args){
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services
            .AddAuthentication().AddScheme<AuthenticationSchemeOptions, A2AuthHandler>("MyAuthentication", null);
            
            builder.Services.AddDbContext<A2DbContext>(options =>
     options.UseSqlite(builder.Configuration["A2DBConnection"]));

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("UserOnly", policy => policy.RequireClaim(ClaimTypes.Role, "RegisteredUser"));
            options.AddPolicy("OrganiserOnly", policy => policy.RequireClaim(ClaimTypes.Role, "RegisteredOrganiser"));
            options.AddPolicy("AuthOnly", policy => policy.RequireAssertion(context => context.User.HasClaim(c=>(c.Value == "RegisteredUser" || c.Value == "RegisteredOgraniser"))));
        });


        builder.Services.AddScoped<IA2Repo, A2Repo>();
        builder.Services.AddMvc(options => options.OutputFormatters.Add(new CalendarOuputFormatter()));
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
        }
    }
}