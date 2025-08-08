using Microsoft.EntityFrameworkCore;
using CozyComfortAPI.Data;
using Microsoft.OpenApi.Models;
using CozyComfortAPI.Auth; 
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
var conn = "Data Source=HARSHAMAX_PC;Initial Catalog=CozyComfort;Integrated Security=True;TrustServerCertificate=True";
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(conn));
builder.Services.AddScoped<BlanketModelRepo>();
builder.Services.AddControllers();

//Authentication services
builder.Services.AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationOptions.DefaultScheme,
        options => { });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CozyComfortAPI", Version = "v1" });

    //API Key security scheme
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key authorization using X-API-KEY header (Example: 'X-API-KEY YOUR_API_KEY')",
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });


    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
            },
            new string[] { }
        }
    });
});

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