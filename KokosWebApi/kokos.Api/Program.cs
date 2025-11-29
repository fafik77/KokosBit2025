using kokos.Api.Extensions;
using kokos.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

#region Init App
// use args to init the app
var builder = WebApplication.CreateBuilder(args);
// connect to the DB, `DefaultConnection` is a secret or EnvVal
var DefaultConnectionString = builder.Configuration["DefaultConnection"];
#endregion //Init App

#region CORS Access-Control-Allow-Origin
//var AllowSpecificOrigins = "_AllowSpecificOrigins";
var AllowAllOrigins = "_AllowAllOrigins";
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: AllowAllOrigins,
					  policy =>
					  {
						  policy.AllowAnyOrigin()
						  .AllowAnyHeader()
						  .AllowAnyMethod();
					  });
});
#endregion

#region Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add Swagger	Authorization: https://www.youtube.com/watch?v=auGpCZRiEtA
builder.Services.AddSwaggerGen(opt =>
{
	opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Enter a valid Token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
});


var ExecutingAssembly = Assembly.GetExecutingAssembly();
builder.Services.AddAutoMapper(ExecutingAssembly);  ///Important breaking changes: since 15.0+ registration and purchase is required so you have to use 14.0  https://docs.automapper.io/en/stable/15.0-Upgrade-Guide.html

//add Identity	https://www.youtube.com/watch?v=S0RSsHKiD6Y, https://www.youtube.com/watch?v=4cFhYUK8wnc&list=PLYpjLpq5ZDGtJOHUbv7KHuxtYLk1nJPw5&index=1
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
	{
		// Set the Bearer token scheme as the default for both
		// authentication (checking credentials) and challenge (asking to log in).
		options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
		options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
	})
	.AddJwtBearer()
	.AddCookie(IdentityConstants.ApplicationScheme)
	.AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddIdentityCore<User>()
	.AddEntityFrameworkStores<UserDBContext>()
	.AddApiEndpoints();
#endregion // Add services to the container.

// Add DB for todos and Users
builder.Services.AddDbContext<TodoDBContext>(opt =>
	opt.UseNpgsql(DefaultConnectionString));
builder.Services.AddDbContext<UserDBContext>(opt =>
	opt.UseNpgsql(DefaultConnectionString));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();

	// Apply DB Migrations on start
	await app.ApplyMigrationsAsync();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins"); // Reference the policy name defined above

app.UseAuthorization();
app.MapIdentityApi<User>();

app.MapControllers();

app.Run();
