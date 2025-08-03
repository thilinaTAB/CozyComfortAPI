using Microsoft.EntityFrameworkCore;
using CozyComfortAPI.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var conn = "Data Source=HARSHAMAX_PC;Initial Catalog=CozyComfort;Integrated Security=True;TrustServerCertificate=True";
builder.Services.AddDbContext<AppDbContext>(op=>op.UseSqlServer(conn));
builder.Services.AddScoped<BlanketModelRepo>();
//Add services to the container
builder.Services.AddControllers();


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CozyComfortAPI", Version = "v1" });

    // Define the API Key security scheme
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
