using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using BACK_END_PROJECT.API;
using BACK_END_PROJECT.API.users;
using BACK_END_PROJECT.API.security;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container de DI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter your Bearer token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configuração do DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração de bind da seção "security.admin" no appsettings.json para AdminConfig
builder.Services.Configure<AdminConfig>(builder.Configuration.GetSection("security:admin"));

// Registrando dependências de JWT e filtro de token
builder.Services.AddScoped<Jwt>(); // Registrar o Jwt
builder.Services.AddScoped<JwtTokenFilter>();

// Adicionando o Bootstrapper
builder.Services.AddSingleton<Bootstrapper>();

var app = builder.Build();

// Inicializando o Bootstrapper assincronamente
var bootstrapper = app.Services.GetRequiredService<Bootstrapper>();
await bootstrapper.InitializeAsync(); // Esperar a inicialização assíncrona

// Configuração do Swagger e outros middlewares em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuração do middleware
app.UseHttpsRedirection();

// Registrando os middlewares de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Registrando o filtro de JWT
app.UseMiddleware<JwtTokenFilter>();

// Mapeamento de controladores (endpoints API)
app.MapControllers();

app.Run();
