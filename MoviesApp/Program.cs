using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using MoviesApi.Models.Dtos;
using MoviesApi.Models.Validators;
using MoviesApi.Middleware;
using MoviesApi.Services;
using MoviesApp;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MoviesApp.Models.Dtos;
using MoviesApp.Models.Validators;
using MoviesApp.Services;
using Microsoft.AspNetCore.Identity;
using MoviesApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer"; //DefaultAuthenticationScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            authenticationSettings.JwtKey)),
    };
});
builder.Services.AddAuthorization(options =>
{
});

builder.Services.AddScoped<AutomaticMigrations>();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MoviesDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")));
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddScoped<IValidator<CreateMovieDto>, CreateMovieDtoValidator>();
builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
builder.Services.AddScoped<IValidator<MovieQuery>, MovieQueryValidator>();
builder.Services.AddScoped<IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);



builder.Services.AddCors(options =>
{
    options.AddPolicy("MainPolicy", builder =>
    builder.AllowAnyMethod()
    //.WithOrigins(builder.Configuration["AllowedOrigins"])
    //.WithOrigins("http://localhost:7115")
    .AllowAnyOrigin()
    .AllowAnyHeader()
    );
});


var app = builder.Build();

void GetPendingMigrations()
{
    using (var scope = app.Services.CreateScope())
    {
        var migrator = scope.ServiceProvider.GetRequiredService<AutomaticMigrations>();
        migrator.GetMigrations();
    }
}

// Configure the HTTP request pipeline.
app.UseCors("Localhost");
GetPendingMigrations();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "MoviesApp");
});
app.UseMiddleware<ErrorHandlingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
