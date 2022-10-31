using AuthenticationNet6;
using AuthenticationNet6.Models;
using EFDataAccess;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings.Authentication>(builder.Configuration.GetSection("authentication"));

// Add services to the container.
ContainerHelper.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
	option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Auth token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id="Bearer"
				}
			},
			new string[]{}
		}
	});
});


var app = builder.Build();

app.UseMiddleware<AuthenticationMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	//using (var scope = app.Services.CreateScope())
	//{
	//	var dbContext = scope.ServiceProvider.GetRequiredService<EFAppContext>();
	//	dbContext.Database.EnsureDeleted();
	//	dbContext.Database.EnsureCreated();
	//}

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
