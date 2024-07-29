using DemoTokenAPI.Services;
using DemoTokenAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<UserService>();
builder.Services.AddScoped<TokenManager>();

string _secretKey = builder.Configuration.GetSection("tokenInfo").GetSection("secret").Value;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
            ValidateLifetime = true,
            ValidAudience = "https://monsite.com",
            ValidIssuer = "https://monapi.com",
            ValidateIssuer = false, //Par défaut à true
            ValidateAudience = false //Par défaut à true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("adminRequired", policy => policy.RequireRole("admin"));
    options.AddPolicy("connected", policy => policy.RequireAuthenticatedUser());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Dans cet ordre précis sinon c'est tout nu dans les orties
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(o => o.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.MapControllers();

app.Run();
