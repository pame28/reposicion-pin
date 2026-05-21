using Microsoft.EntityFrameworkCore;
using reposicion_pin.Infrastructure.Data;
using reposicion_pin.Repository;
using reposicion_pin.Repository.Interface;
using reposicion_pin.Service;
using reposicion_pin.Service.Interface;
using reposicion_pin.Utils;
using Serilog;


var builder = WebApplication.CreateBuilder(args);



// -----------------------
// Configuración Serilog
// -----------------------
builder.Host.UseSerilog((context, services, config) =>
    config.ReadFrom.Configuration(context.Configuration)
          .ReadFrom.Services(services));

// -----------------------
// Opciones del CorrelationId
// -----------------------
builder.Services.Configure<CorrelationIdOptions>(
    builder.Configuration.GetSection("CorrelationIdOptions"));



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReposicionPinConnection"))
    );
//REPOSITORIES
builder.Services.AddScoped<ITarjetaRepository, TarjetaRepository>();
builder.Services.AddScoped<ICatalogoRespuestaRepository, CatalogoRespuestaRepository>();
builder.Services.AddScoped<IAS400Repository, AS400Repository>();
builder.Services.AddScoped<IConfiguracionAs400Repository, ConfiguracionAs400Repository>();
builder.Services.AddScoped<IOperadorRepository, OperadorRepository>();
builder.Services.AddScoped<IEnvioSmsRepository, EnvioSmsRepository>();
builder.Services.AddScoped<IBandejaEnvioSmsRepository, BandejaEnvioSmsRepository>();
builder.Services.AddScoped<IBandejaEnvioEmailRepository, BandejaEnvioEmailRepository>();
builder.Services.AddScoped<IEnvioEmailRepository, EnvioEmailRepository>();




//SERVICIOS
builder.Services.AddScoped <ITarjetaValidationService, TarjetaValidationService>();
builder.Services.AddScoped<IAs400Service, As400Service>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<ReposicionPinService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCorrelationId();
app.UseAuthorization();
app.MapControllers();

app.Run();
