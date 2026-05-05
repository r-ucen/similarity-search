using System.Threading.RateLimiting;
using Hangfire;
using Scalar.AspNetCore;
using SimilaritySearch.Api;
using SimilaritySearch.Api.Middleware;
using SimilaritySearch.Application;
using SimilaritySearch.Domain;
using SimilaritySearch.Infrastructure;
using SimilaritySearch.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

var storeSettings = builder.Configuration
    .GetSection("CurrencySettings")
    .Get<CurrencySettings>() ?? throw new Exception("CurrencySettings section is missing in json config");;

builder.Services.AddSingleton(storeSettings);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentation();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    if (!builder.Environment.IsDevelopment())
    {
        options.Cookie.Domain = ".rucen.me";
    }
    
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("IdentityPolicy", httpContext =>
        {
            if (httpContext.Request.Method == HttpMethods.Options)
            {
                return RateLimitPartition.GetNoLimiter("preflight");
            }
            
            var path = httpContext.Request.Path.Value ?? "";

            if (path.Contains("/manage/info"))
            {
                return RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: httpContext.User.Identity?.Name +
                                  path +
                                  httpContext.Connection.RemoteIpAddress,
                    factory: _ => new TokenBucketRateLimiterOptions()
                    {
                        TokenLimit = 1000,
                        TokensPerPeriod = 100,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    }
                );
            }
            
            if (path.Contains("/resendConfirmationEmail") || path.Contains("/forgotPassword"))
            {
                return RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: httpContext.User.Identity?.Name +
                                  path +
                                  httpContext.Connection.RemoteIpAddress,
                    factory: _ => new TokenBucketRateLimiterOptions()
                    {
                        TokenLimit = 20,
                        TokensPerPeriod = 1,
                        ReplenishmentPeriod = TimeSpan.FromHours(1),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    }
                );
            }

            if (path.Contains("/login") || path.Contains("/register"))
            {
                return RateLimitPartition.GetFixedWindowLimiter(
                    path +
                    httpContext.Connection.RemoteIpAddress,_ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1)
                });
            }
            
            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 120,
                    Window = TimeSpan.FromMinutes(1)
                }
            );
        }
    );
    
    options.AddPolicy("RoleCheckPolicy", httpContext =>
        {
            if (httpContext.Request.Method == HttpMethods.Options)
            {
                return RateLimitPartition.GetNoLimiter("preflight");
            }

            return RateLimitPartition.GetTokenBucketLimiter(
                partitionKey: httpContext.User.Identity?.Name ??
                              httpContext.Connection.RemoteIpAddress?.ToString() ??
                              "unknown",
                factory: _ => new TokenBucketRateLimiterOptions()
                {
                    TokenLimit = 1000,
                    TokensPerPeriod = 100,
                    ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                    QueueLimit = 0,
                    AutoReplenishment = true
                }
            );
        }
    );

    options.AddPolicy("DataPolicy", httpContext =>
        {
            if (httpContext.Request.Method == HttpMethods.Options)
            {
                return RateLimitPartition.GetNoLimiter("preflight");
            }

            return RateLimitPartition.GetTokenBucketLimiter(
                partitionKey: httpContext.User.Identity?.Name ??
                              httpContext.Connection.RemoteIpAddress?.ToString() ??
                              "unknown",
                factory: _ => new TokenBucketRateLimiterOptions()
                {
                    TokenLimit = 150,
                    TokensPerPeriod = 20,
                    ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                    QueueLimit = 0,
                    AutoReplenishment = true
                }
            );
        }
    );
    
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(new { 
            message = "Rate limit exceeded." 
        }, cancellationToken);
    };
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseHangfireDashboard();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowSpecificOrigins");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<ApplicationUser>().RequireRateLimiting("IdentityPolicy");
app.MapControllers();

app.Run();

public partial class Program { }
