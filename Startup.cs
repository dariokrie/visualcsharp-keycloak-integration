using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Showcasing.Keycloak.Configuration;

namespace Showcasing.Keycloak;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public KeycloakSettings KeycloakSettings { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
        var keyCloakSettings = new KeycloakSettings();
        KeycloakSettings = keyCloakSettings;
        Configuration.GetSection("Keycloak").Bind(keyCloakSettings);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                 options =>
                 {
                     options.Authority = KeycloakSettings.Authority;
                     options.Audience = "account";
                     options.RequireHttpsMetadata = false;
                 });

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{KeycloakSettings.Authority}/protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{KeycloakSettings.Authority}/protocol/openid-connect/token"),
                    }
                }
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new[] { "openid", "profile", "email" }
                }
            });
        });

        services.AddCors(corsOptions => corsOptions.AddPolicy("AllowAllPolicy", policyBuilder =>
        {
            policyBuilder.SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Keycloak Authorization API");
                options.OAuthClientId(KeycloakSettings.ClientId);
                options.OAuthClientSecret(KeycloakSettings.ClientSecret);
            });
        }

        app.UseHttpsRedirection()
            .UseRequestLocalization()
            .UseRouting()
            .UseCors("AllowAllPolicy")
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
    }
}
