//using EvaluateSystems.Configuration;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using skillsphere.core.Interfaces.Services;
//using skillsphere.infrastructure.Services;
//using Swashbuckle.AspNetCore.Filters;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//var configuration = builder.Configuration;

//builder.Services.ConfigureServices(builder.Configuration);
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    // Add JWT Authorization to Swagger
//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Description = "Enter JWT token like: Bearer {your token}",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer"
//    });

//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            new string[] {}
//        }
//    });

//    // Optional: auto adds lock icon for [Authorize] endpoints
//    options.OperationFilter<SecurityRequirementsOperationFilter>();
//});


//// JWT setup
//var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = configuration["Jwt:Issuer"],
//        ValidAudience = configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ClockSkew = TimeSpan.Zero
//    };

//    options.Events = new JwtBearerEvents
//    {
//        OnAuthenticationFailed = context =>
//        {
//            Console.WriteLine("JWT ERROR: " + context.Exception.Message);
//            return Task.CompletedTask;
//        }
//    };
//});


//// Register your JWT service
//builder.Services.AddScoped<IJwtService, JwtService>();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
//});


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseCors("AllowAll");

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();


//app.Run();




using EvaluateSystems.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using skillsphere.core.Interfaces.Services;
using skillsphere.infrastructure.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Register services
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Add JWT Authorization to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT token like: Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
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

    // Optional: Auto adds lock icon for [Authorize] endpoints
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// JWT setup
var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("JWT ERROR: " + context.Exception.Message);
            return Task.CompletedTask;
        }
    };
});

// Register your JWT service
builder.Services.AddScoped<IJwtService, JwtService>();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker") || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// ⚠️ Remove HTTPS redirection for Docker (no SSL cert inside container)
// app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ✅ Ensure app listens on all network interfaces in Docker
app.Urls.Add("http://0.0.0.0:8080");

app.Run();
