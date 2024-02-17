using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Security;

namespace ChatRoom.Framework.Configuration
{
	public class AuthConfiguration
	{
		public const string SECTION_NAME = "Auth";

		public string SigningKey { get; set; }
		public string DecryptionKey { get; set; }
		public string Issuer { get; set; }
		public bool ValidateIssuer { get; set; }
		public string Audience { get; set; }
		public bool ValidateAudience { get; set; }
		public int ExpiresMinute { get; set; } = 15;

		public static AuthConfiguration FromEnviroment()
		{
			var config = new AuthConfiguration();
			try
			{
				config.Issuer = ChatRoomEnviroment.ReadVariable(ChatRoomEnviroment.AUTH_ISSUER);
				config.Audience = ChatRoomEnviroment.ReadVariable(ChatRoomEnviroment.AUTH_AUD);
				config.ValidateAudience = ChatRoomEnviroment.ReadVariable(ChatRoomEnviroment.AUTH_VAUD) == "true";
				config.ValidateIssuer = ChatRoomEnviroment.ReadVariable(ChatRoomEnviroment.AUTH_VISSUER) == "true";
				config.DecryptionKey = ChatRoomEnviroment.ReadVariable(ChatRoomEnviroment.AUTH_DKEY);
				config.SigningKey = ChatRoomEnviroment.ReadVariable(ChatRoomEnviroment.AUTH_SKEY);
				config.ExpiresMinute = int.Parse(ChatRoomEnviroment.ReadVariable(ChatRoomEnviroment.AUTH_EXP));
			}
			catch (ArgumentNullException)
			{
				throw new Exception("Authentication Variable");
			}
			catch (SecurityException)
			{
				throw new Exception("Authentication Variable");
			}

			return config;
		}

		public static AuthConfiguration FromConfiguration(IConfiguration configuration)
		{
			IConfigurationSection section;
			try
			{
				section = configuration.GetRequiredSection(SECTION_NAME);
			}
			catch (InvalidOperationException)
			{
				throw new Exception(SECTION_NAME);
			}

			try
			{
				var config = new AuthConfiguration();
				new ConfigureFromConfigurationOptions<AuthConfiguration>(section)
				   .Configure(config);
				return config;
			}
			catch (Exception)
			{
				throw new Exception(SECTION_NAME);
			}
		}
	}

}
