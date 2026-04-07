using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce.Presentation.Extentions
{
    public static class OpenApiScalarExtensions
    {
        public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    // 1. المعلومات الأساسية
                    document.Info = new OpenApiInfo
                    {
                        Title = "E-commerce for Kids API",
                        Version = "v1",
                        Description = "API documentation for the Kids E-commerce application.",
                        Contact = new OpenApiContact
                        {
                            Name = "Sobieh Support",
                            Email = "sobihmohamedsobih@gmail.com"
                        }
                    };

                    // 2. تعريف نظام الأمان (JWT Setup)
                    var securityScheme = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Enter your JWT Token here."
                    };

                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                    {
                        ["Bearer"] = securityScheme
                    };

                    // 3. تفعيل القفل على كل الـ Endpoints
                    document.Security ??= new List<OpenApiSecurityRequirement>();
                    document.Security.Add(new OpenApiSecurityRequirement
                    {
                        // 👈 حلينا مشكلة الإيرور التالت هنا: استخدمنا new List<string>() بدل الـ Array
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                    });

                    return Task.CompletedTask;
                });
            });

            return services;
        }

        public static IEndpointRouteBuilder MapScalarDocumentation(this IEndpointRouteBuilder app)
        {
            app.MapOpenApi();

            app.MapScalarApiReference(options =>
            {
                options.WithTitle("E-Commerce Kids API")
                       .WithTheme(ScalarTheme.DeepSpace)
                       .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });

            return app;
        }
    }
}