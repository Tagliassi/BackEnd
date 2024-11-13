using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using BACK_END_PROJECT.API;
using BACK_END_PROJECT.API.users;
using BACK_END_PROJECT.API.roles;
using BACK_END_PROJECT.API.security;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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

// Configuração do DbContext para MySQL
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new ArgumentNullException("builder.Configuration.GetConnectionString(\"DefaultConnection\")");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4, 2)));
});

// Configuração de bind da seção "security.admin" no appsettings.json para AdminConfig
builder.Services.Configure<AdminConfig>(builder.Configuration.GetSection("security:admin"));



// Registrando dependências de JWT e filtro de token
builder.Services.AddScoped<Jwt>(); // Registrar o Jwt
builder.Services.AddScoped<JwtTokenFilter>(); // Registrar o filtro de JWT

// Registrando repositórios e serviços
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();

// Configuração de autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "PUCPR AuthServer", // Atualize conforme necessário
            ValidAudience = "PUCPR AuthServer", // Atualize conforme necessário
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0e5582adfb7fa6bb770815f3c6b3534d311bd5fe")) // Substitua pela sua chave secreta real
        };
    });
// Adicionando serviços para autorização e controladores
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Adicionando o Bootstrapper
builder.Services.AddScoped<Bootstrapper>();

var app = builder.Build();

// Criando um escopo manual para inicializar o Bootstrapper
using (var scope = app.Services.CreateScope())
{
    var bootstrapper = scope.ServiceProvider.GetRequiredService<Bootstrapper>();
    await bootstrapper.InitializeAsync(); // Esperar a inicialização assíncrona
}

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

// Mapeamento de controladores (endpoints API)
app.MapControllers();

app.Run();
