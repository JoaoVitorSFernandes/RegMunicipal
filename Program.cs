using System.Text;
using DesafioBalta;
using DesafioBalta.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>();

builder.Services.AddSwaggerGen( c => {
    c.SwaggerDoc("v1", new OpenApiInfo{Title = "RegMunicipal", Version = "v1"});
});

builder.Services.AddControllers();

var key = Encoding.ASCII.GetBytes(Settings.PrivateKey);

builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {

    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters{
 
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddAuthorization(x =>{
    x.AddPolicy("Admin", policy => policy.RequireRole("manager"));
    x.AddPolicy("Employee", policy => policy.RequireRole("employee"));
});

var app = builder.Build();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RegMunicipal v1"));

app.Run();
