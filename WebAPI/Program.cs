using DAL;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.GetApplicationDefault(),
});
builder.Services.AddDbContext<BlogSiteDbContext>(
    options => options.UseSqlServer(Environment.GetEnvironmentVariable("")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = "https://securetoken.google.com/hotiendat-blog";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "https://securetoken.google.com/hotiendat-blog",
        ValidateAudience = true,
        ValidAudience = "hotiendat-blog",
        ValidateLifetime = true,
    };
});

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();