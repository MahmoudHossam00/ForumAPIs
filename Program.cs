
using BlogProject.Data;
using BlogProject.Filteers;
using BlogProject.Filters;

//using BlogProject.Filters;
using BlogProject.Helpers;
using BlogProject.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;

namespace BlogProject
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers()
				.AddJsonOptions(options =>
			{
				
				options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
			});
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();


			builder.Services.AddScoped<AuthorFilterAttribute>();
			builder.Services.AddScoped<AuthorAndAdminsFilterAttribute>();
			builder.Services.AddTransient<ICategoryService, CategoryService>();
			builder.Services.AddTransient<IPostService, PostService>();
			builder.Services.AddTransient<ICommentService, CommentsService>();
			builder.Services.AddTransient<IReplyService, ReplyService>();
			builder.Services.AddAutoMapper(typeof(Program));
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.Configure<rolesconfigs>(builder.Configuration.GetSection("RolesConfig"));

			builder.Services.Configure<PictureSpecifications>(builder.Configuration.GetSection("PictureSpecifications"));
			builder.Services.AddAuthorization(options =>
			options.AddPolicy("ModsAndAdmins",builder =>  builder.RequireAssertion(
				context=>
				{
					return context.User.IsInRole("ADMIN") || context.User.IsInRole("Moderator");
				}
				)));
			builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
			//builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
			var jwtOptions = builder.Configuration.GetSection("JWT").Get<JWT>();
			builder.Services.AddDbContext<ApplicationDbContext>(options=> 
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>();
			
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(o =>
			{
				o.RequireHttpsMetadata = true;
				o.SaveToken = true;
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidIssuer = builder.Configuration["JWT:Issuer"],
					ValidAudience = jwtOptions.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))

				};
			});

			builder.Services.AddCors();
			builder.Services.AddSwaggerGen();
			var app = builder.Build();


			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
			app.UseAuthentication();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
