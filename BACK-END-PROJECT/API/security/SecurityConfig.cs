using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BACK_END_PROJECT.API.security
{
    public class SecurityConfig
    {
        private readonly IConfiguration _configuration;

        public SecurityConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuração de CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            // Obter a chave secreta e outras configurações do appsettings.json
            var jwtSettings = _configuration.GetSection("Jwt");
            var secret = jwtSettings["Secret"];
            var issuer = jwtSettings["Issuer"];
            var expireHours = int.Parse(jwtSettings["ExpireHours"]);

            // Adicionar autenticação JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;  // Set to true in production
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = issuer,
                        ValidAudience = issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                        ClockSkew = TimeSpan.Zero // Remove clock skew for exact expiration time
                    };
                });

            // Configuração do Swagger
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("AuthServer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "AuthServer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Adicionar filtros e políticas de segurança adicionais
            services.AddScoped<JwtTokenFilter>();
        }

        public void Configure(IApplicationBuilder app)
        {
            // Usando CORS
            app.UseCors();

            // Usar autenticação JWT
            app.UseAuthentication();

            // Usar autorização
            app.UseAuthorization();

            // Configuração do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Server API");
                c.RoutePrefix = string.Empty;
            });

            // Filtros de segurança
            app.UseMiddleware<JwtTokenFilter>();  // Filtro de JWT personalizado

            // Adicionar suporte ao H2 console (se necessário)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // Mapeando o caminho H2 Console corretamente
                endpoints.Map("/h2-console/**", async context =>
                {
                    context.Response.Redirect("https://localhost:8082/h2-console", permanent: false); // Redireciona para o H2 Console
                }).AllowAnonymous();
            });
        }
    }
}
