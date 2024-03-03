using ChatRoom.Application;
using ChatRoom.Framework.Configuration;
using ChatRoom.Infrastracture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
var fromFile = builder.Environment.IsDevelopment()
	|| ChatRoomEnviroment.IsTestEnvironment()
	|| ChatRoomEnviroment.IsFromSettingFile();

builder.Services
	.AddChatIdentity(fromFile ? configuration : null)
	.AddApplicationService(fromFile ? configuration : null)
	.AddRepository(fromFile ? configuration : null);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
