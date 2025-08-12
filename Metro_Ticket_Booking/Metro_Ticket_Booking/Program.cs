//using System.Text.Json.Serialization;
//using Metro_Ticket_Booking.Models;
//using Metro_Ticket_Booking.Services;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// Name for CORS policy
//var corsPolicyName = "AllowFrontend";

//// Add services to the container.

//// Configure DbContext with connection string from appsettings.json
//builder.Services.AddDbContext<MetroTicketContext>(options =>
//    options.UseMySql(
//        builder.Configuration.GetConnectionString("MetroTicketDb"),
//        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MetroTicketDb"))
//    ));

//// Configure Authentication Service
//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<IAdminService, AdminService>();
//builder.Services.AddScoped<IUserService, UserService>();

////payment service 
//builder.Services.AddScoped<PaymentService>();


//// Add Controllers support with JSON options to handle circular references
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//        // or use ReferenceHandler.Preserve if you want to support reference metadata
//        // options.JsonSerializerOptions.MaxDepth = 64; // optional increase max depth
//    });

//// Configure CORS to allow frontend origin
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: corsPolicyName,
//        policy =>
//        {
//            policy
//                .WithOrigins(
//                                "https://689a83dbceafad905e6e7790--metro-one.netlify.app/"   // React dev server URL - change if needed
//                            )
//                .AllowAnyHeader()
//                .AllowAnyMethod()
//                .AllowCredentials();

//        });
//});

//// Configure JWT authentication
//var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//var secretKey = jwtSettings["SecretKey"];

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,

//        ValidIssuer = jwtSettings["Issuer"],
//        ValidAudience = jwtSettings["Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

//        ClockSkew = TimeSpan.Zero
//    };
//});

//// Add Authorization
//builder.Services.AddAuthorization();

//// Enable Swagger for API documentation
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Middleware pipeline configuration
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//// Enable CORS with the named policy
//app.UseCors(corsPolicyName);

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();



using System.Text.Json.Serialization;
using Metro_Ticket_Booking.Models;
using Metro_Ticket_Booking.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Name for CORS policy
var corsPolicyName = "AllowAll";

// Add services to the container.

// Configure DbContext with connection string from appsettings.json
builder.Services.AddDbContext<MetroTicketContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MetroTicketDb"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MetroTicketDb"))
    ));

// Configure Authentication Service
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserService, UserService>();

// Payment service 
builder.Services.AddScoped<PaymentService>();

// Add Controllers support with JSON options to handle circular references
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Configure CORS to allow all origins, headers, and methods
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            // Do NOT call AllowCredentials() when AllowAnyOrigin() is used as it causes runtime error
        });
});

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

        ClockSkew = TimeSpan.Zero
    };
});

// Add Authorization
builder.Services.AddAuthorization();

// Enable Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS with the named policy
app.UseCors(corsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();