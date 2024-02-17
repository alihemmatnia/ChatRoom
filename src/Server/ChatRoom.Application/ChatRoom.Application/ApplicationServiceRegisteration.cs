using ChatRoom.Application.Services;
using ChatRoom.Application.Services.Interfaces;
using ChatRoom.Framework.Configuration;
using ChatRoom.Framework.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ChatRoom.Application
{
	public static class ApplicationServiceRegisteration
	{
		public static void AddApplicationService(this IServiceCollection services, IConfiguration? configuration = null)
		{

			services.AddScoped<IUserApplicationService, UserApplicationService>();
		}


		public static IServiceCollection AddChatIdentity(this IServiceCollection services,
			IConfiguration? configuration = null)
		{
			if (configuration == null)
				ChatRoomAuthentication.Configuration = AuthConfiguration.FromEnviroment();
			else
				ChatRoomAuthentication.Configuration = AuthConfiguration.FromConfiguration(configuration);

			ChatRoomAuthentication.Configure();

			services.AddHttpContextAccessor();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = ChatRoomAuthentication.Configuration.ValidateIssuer,
					ValidateAudience = ChatRoomAuthentication.Configuration.ValidateAudience,
					ValidAudience = ChatRoomAuthentication.Configuration.Audience,
					ValidIssuer = ChatRoomAuthentication.Configuration.Issuer,
					TokenDecryptionKey = ChatRoomAuthentication.AuthDecryptionKey,
					IssuerSigningKey = ChatRoomAuthentication.AuthSigningKey,
					ValidateLifetime = true,
					// default is 5 mins, so when we were setting the expiration to 15 mins it was actually 20 mins
					ClockSkew = TimeSpan.Zero
				};
				options.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = context =>
					{
						if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
						{
							context.Response.Headers.Add("token-expired", "true");
						}
						return Task.CompletedTask;
					}
				};
			});

			services.AddAuthorization();

			return services;
		}
	}
}
