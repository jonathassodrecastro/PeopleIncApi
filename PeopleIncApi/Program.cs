using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PeopleIncApi.Data;
using PeopleIncApi.Interfaces;
using PeopleIncApi.Repositories;
using PeopleIncApi.Services;
using System.Reflection;

var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PeopleIncAPI", Version = "v1" });

    // Especifique o caminho para o arquivo XML de documentação
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddSingleton(configuration);

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<IPessoaService, PessoaService>();

builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.AddHttpContextAccessor();
//config arquivo csv
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = 1024 * 1024; // 1MB
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PeopleIncAPI V1");
    });
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
