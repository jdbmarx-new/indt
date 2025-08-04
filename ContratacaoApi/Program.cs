using Common.Domain.Interfaces;
using ContratacaoApi.Application.Interfaces;
using ContratacaoApi.Domain.Services;
using ContratacaoApi.Infrastructure;
using ContratacaoApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddDbContext<ContratacaoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
    //options.UseInMemoryDatabase("ContratacaoDb") // For testing purposes, using InMemory database
    );

services.AddScoped<IContratacaoService, ContratacaoService>();
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IContratacaoRepository, ContratacaoRepository>();
services.AddHttpClient<IPropostaServiceAgent, PropostaServiceAgent>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("PropostaService:BaseUrl")!);
});

services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Migration code is commented out for simplicity, but can be used if needed

using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    var context = sp.GetRequiredService<ContratacaoDbContext>();
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();