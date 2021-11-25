using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Persistencia;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Aplicacion.Cursos;
using FluentValidation.AspNetCore;
using WebAPI.Middleware;
using Dominio;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Aplicacion.Contratos;
using Seguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Configure Services
builder.Services.AddDbContext<CursosOnlineContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMediatR(typeof(Consulta.Manejador).Assembly);

builder.Services.AddControllers( opt => 
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
})
.AddFluentValidation( cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

var p = builder.Services.AddIdentityCore<Usuario>();
var IdentityBuilder = new IdentityBuilder(p.UserType, p.Services);
IdentityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
IdentityBuilder.AddSignInManager<SignInManager<Usuario>>();
IdentityBuilder.AddSignInManager<SignInManager<Usuario>>();
builder.Services.TryAddSingleton<ISystemClock, SystemClock>();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

builder.Services.AddScoped<IJwtGenerador, JwtGenerador>();

builder.Services.AddScoped<IUsuarioSesion, UsuarioSesion>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

var app = builder.Build();

#region Configure Provider
// Configure the HTTP request pipeline.
app.UseMiddleware<ManejadorErrorMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
#endregion

#region Program
using var scope = app.Services.CreateScope();
var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();
var context = scope.ServiceProvider.GetRequiredService<CursosOnlineContext>();
await context.Database.MigrateAsync();
await DataPrueba.InsertarData(context, UserManager);


//await scope.ServiceProvider.GetService<CursosOnlineContext?>().Database.MigrateAsync();
app.Run();
#endregion