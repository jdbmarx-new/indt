using Common.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PropostaApi.Application.Interfaces;
using PropostaApi.Domain.Services;
using PropostaApi.Infrastructure;
using PropostaApi.Infrastructure.Persistence;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<PropostaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
    //options.UseInMemoryDatabase("PropostaDb") // For testing purposes, using InMemory database
    );

services.AddScoped<IPropostaService, PropostaService>();
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IPropostaRepository, PropostaRepository>();

services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
});

var app = builder.Build();

// Migration code is commented out for simplicity, but can be used if needed

using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    var context = sp.GetRequiredService<PropostaDbContext>();
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();