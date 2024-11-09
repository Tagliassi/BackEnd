using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BACK_END_PROJECT.API.security
{
    public class SecurityConfig
    {
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
                        ValidIssuer = Jwt.SECRET,
                        ValidAudience = Jwt.SECRET,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt.SECRET))
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

            // Adicionar filtros e políticas de segurança adicionais, como um filtro CORS
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

            // Configuração de Swagger
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
                endpoints.Map("/h2-console/**").AllowAnonymous();
            });
        }
    }
}
