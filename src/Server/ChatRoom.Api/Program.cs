using ChatRoom.Application;
using ChatRoom.Framework.Configuration;
using ChatRoom.Infrastracture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
var fromFile = builder.Environment.IsDevelopment()
	|| ChatRoomEnviroment.IsTestEnvironment()
	|| ChatRoomEnviroment.IsFromSettingFile();


builder.Services
	.AddRepository(fromFile ? configuration : null);

builder.Services
	.AddApplicationService(fromFile ? configuration : null);

builder.Services
	.AddChatIdentity(fromFile ? configuration : null);

var app = builder.Build();

	app.UseSwagger();
	app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
