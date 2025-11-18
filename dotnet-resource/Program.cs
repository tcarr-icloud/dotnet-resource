using System.Text.Json.Serialization;
using dotnet_resource.FlatNode;
using dotnet_resource.TreeNode;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<FlatNodeDbContext>(options => 
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.Authority = configuration["Authority"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidAudiences = configuration.GetSection("ValidAudiences").Get<List<string>>()
        };
    });

builder.Services.AddAuthorizationBuilder();

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<ITreeNodeService, TreeNodeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.Run();