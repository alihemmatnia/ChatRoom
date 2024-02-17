using ChatRoom.Framework.Configuration;
using ChatRoom.Infrastracture.Context;
using ChatRoom.Infrastracture.Repositories;
using ChatRoom.Infrastracture.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRoom.Infrastracture
{
	public static class RepositoryRegisteration
	{
		public static void AddRepository(this IServiceCollection services,
		   IConfiguration? configuration = null)
		{

			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IUnitOfWork<ChatRoomDbContext>, UnitOfWork<ChatRoomDbContext>>();


			services.AddDbContext<ChatRoomDbContext>(op =>
			{
				op.UseSqlServer(configuration != null ? configuration.GetConnectionString("dbConn") : ChatRoomEnviroment.ReadVariable(ChatRoomEnviroment.DB_CONN));
			});
		}
	}
}
