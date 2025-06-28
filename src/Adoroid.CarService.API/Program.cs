using Adoroid.CarService.API.Endpoints;
using Adoroid.CarService.Application;
using Adoroid.CarService.Infrastructure;
using Adoroid.CarService.Infrastructure.Auth;
using Adoroid.CarService.Infrastructure.Auth.MobileUser;
using Adoroid.CarService.Infrastructure.Logging;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Exceptions.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7290);
});

builder.Services.AddCarServicePersistence(builder.Configuration);
builder.Services.AddCarServiceApplication(builder.Configuration);
builder.Services.AddCarServiceInsfrastructure(builder.Configuration);
#region token_options
builder.Services.Configure<TokenOptions>(
    builder.Configuration.GetSection(nameof(TokenOptions)));

builder.Services.Configure<MobileTokenOptions>(
    builder.Configuration.GetSection(nameof(MobileTokenOptions)));

var tokenOptions = builder.Configuration.GetSection(nameof(TokenOptions)).Get<TokenOptions>() 
    ?? throw new InvalidOperationException("TokenOptions configuration section is missing or malformed.");

var mobileTokenOptions = builder.Configuration.GetSection(nameof(MobileTokenOptions)).Get<MobileTokenOptions>() 
    ?? throw new InvalidOperationException("MobileTokenOptions configuration section is missing or malformed.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            IssuerSigningKey = SignHandler.GetSecurityKey(tokenOptions.SecurityKey),
            ClockSkew = TimeSpan.Zero
        };
    })
    .AddJwtBearer("MobileUser", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = mobileTokenOptions.Issuer,
            ValidAudience = mobileTokenOptions.Audience,
            IssuerSigningKey = SignHandler.GetSecurityKey(mobileTokenOptions.SecurityKey),
            ClockSkew = TimeSpan.Zero
        };
    });


#endregion

#region redis_options
/*builder.Services.Configure<RedisConfig>(builder.Configuration.GetSection(nameof(RedisConfig)));
var redisConfig = builder.Configuration.GetSection(nameof(RedisConfig)).Get<RedisConfig>() 
    ?? throw new InvalidOperationException("Redis configuration section is missing or malformed.");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = $"{redisConfig.Host}:{redisConfig.Port},password={redisConfig.Password}";
});*/
#endregion


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
app.MainServiceEndpoint();
app.SubServiceEndpoint();
app.SupplierEndpoint();
app.VehicleEndpoint();
app.MasterServiceEndpoint();
app.CompanyServiceEndpoints();
app.AccountTransactionEndpoint();
app.MobileUserEndpoints();
app.GeographicEndpoints();

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
