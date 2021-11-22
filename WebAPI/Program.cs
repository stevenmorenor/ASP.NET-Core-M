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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Configure Services
builder.Services.AddDbContext<CursosOnlineContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMediatR(typeof(Consulta.Manejador).Assembly);

builder.Services.AddControllers().AddFluentValidation( cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

var p = builder.Services.AddIdentityCore<Usuario>();
var IdentityBuilder = new IdentityBuilder(p.UserType, p.Services);
IdentityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
IdentityBuilder.AddSignInManager<SignInManager<Usuario>>();
IdentityBuilder.AddSignInManager<SignInManager<Usuario>>();
builder.Services.TryAddSingleton<ISystemClock, SystemClock>();



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