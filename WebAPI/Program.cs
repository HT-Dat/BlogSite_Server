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

builder.Services.AddDbContext<BlogSiteDbContext>(
    options => options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")));
// Firebase init will cause error on ef migrations because missing env like FIREBASE_SECRET which made the API to crash
// Need to comment this section before doing anythings related to ef migrations
if (Environment.GetEnvironmentVariable("FIREBASE_SECRET").IsNullOrEmpty() == false)
{
    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("FIREBASE_SECRET")),
    });
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
}

// End of Init Firebase Auth
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BlogSiteDbContext>();
    context.Database.Migrate();
}
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