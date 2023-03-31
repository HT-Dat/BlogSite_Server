using BLL;
using BLL.Services;
using BLL.Services.IServices;
using DAL.Persistence;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterBllDependencies(builder.Configuration);

builder.Services.AddDbContext<BlogSiteDbContext>(
    options => options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")));
if (Environment.GetEnvironmentVariable("FIREBASE_SECRET").IsNullOrEmpty() == false)
{
    var cred = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("FIREBASE_SECRET"));
    FirebaseApp.Create(new AppOptions()
    {
        Credential = cred,
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
    var storageClient = StorageClient.Create(cred);
    builder.Services.AddSingleton(storageClient);
}

// End of Init Firebase Auth
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BlogSiteDbContext>();
    context.Database.Migrate();
    await Seed.SeedData(context);

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
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.MapControllers();

app.Run();