using DesafioBalta.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>();

builder.Services.AddSwaggerGen( c => {
    c.SwaggerDoc("v1", new OpenApiInfo{Title = "RegMunicipal", Version = "v1"});
});

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RegMunicipal v1"));

app.Run();
