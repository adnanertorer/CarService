using Adoroid.CarService.API.Endpoints;
using Adoroid.CarService.Application;
using Adoroid.CarService.Infrastructure;
using Adoroid.CarService.Infrastructure.Auth;
using Adoroid.CarService.Infrastructure.Logging;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Exceptions.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7290);
});

builder.Services.AddCarServicePersistence(builder.Configuration);
builder.Services.AddCarServiceInsfrastructure();
builder.Services.Configure<TokenOptions>(
    builder.Configuration.GetSection("TokenOptions"));

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>() 
    ?? throw new InvalidOperationException("TokenOptions configuration section is missing or malformed.");

builder.Services
               .AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
               })
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
               {
                   options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                   {
                       ValidateAudience = true,
                       ValidateIssuer = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = tokenOptions?.Issuer,
                       ValidAudience = tokenOptions?.Audience,
                       IssuerSigningKey = SignHandler.GetSecurityKey(tokenOptions!.SecurityKey),
                       ClockSkew = TimeSpan.Zero
                   };
               });

builder.Services.AddCarServiceApplication(builder.Configuration);

builder.Services.AddHttpContextAccessor();


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecuritySheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecuritySheme, Array.Empty<string>() }
                });
});

builder.Services.AddAuthorization();

builder.Services.AddLoggingAndMonitoring(builder);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseSerilogRequestLogging();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CarServiceDbContext>();
    db.Database.Migrate();
}

app.CompanyEndpoints();
app.UserEndpoints();
app.CustomerEndpoint();
app.EmployeeEndpoint();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseCors("CorsPolicy");

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
