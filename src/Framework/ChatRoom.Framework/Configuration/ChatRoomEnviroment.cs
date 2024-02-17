using System.Security;

namespace ChatRoom.Framework.Configuration
{
	public class ChatRoomEnviroment
	{
		public const string AUTH_ISSUER = "AUTH_ISSUER";
		public const string AUTH_AUD = "AUTH_AUD";
		public const string AUTH_VAUD = "AUTH_VAUD";
		public const string AUTH_VISSUER = "AUTH_VISSUER";
		public const string AUTH_DKEY = "AUTH_DKEY";
		public const string AUTH_EXP = "AUTH_EXP";
		public const string AUTH_SKEY = "AUTH_SKEY";


		public const string DB_CONN = "DB_CONN";

		public static string ReadVariable(string name)
		{
			try
			{
				return Environment.GetEnvironmentVariable(name);
			}
			catch (ArgumentNullException)
			{
				return null;
			}
			catch (SecurityException)
			{
				return null;
			}
		}

		public static T? ReadVariable<T>(string name)
		{
			var value = ReadVariable(name);
			if (value == null) return default(T);

			return ChangeType<T>(value);
		}

		public static T ChangeType<T>(object value)
		{
			var t = typeof(T);

			if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				if (value == null)
				{
					return default(T);
				}

				t = Nullable.GetUnderlyingType(t);
			}

			return (T)Convert.ChangeType(value, t);
		}

		public static string ReadRequiredVariable(string name)
		{
			try
			{
				var value = Environment.GetEnvironmentVariable(name);
				if (value == null) throw new ArgumentNullException();
				return value;
			}
			catch (ArgumentNullException)
			{
				throw new Exception($"Environment Variable {name} Not Provided");
			}
			catch (SecurityException)
			{
				throw new Exception($"SecurityException - Environment Variable {name} Not Found");
			}
		}

		public static T ReadRequiredVariable<T>(string name)
		{
			return (T)Convert.ChangeType(ReadRequiredVariable(name), typeof(T));
		}

		public static bool IsFromSettingFile()
		{
			try
			{
				return Environment.GetEnvironmentVariable("FromFile") == "true";
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool IsTestEnvironment()
		{
			try
			{
				return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test";
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool IsProductionEnvironment()
		{
			try
			{
				return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?
					.Equals("Production", StringComparison.InvariantCultureIgnoreCase) ?? false;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool IsDevelopmentEnvironment()
		{
			try
			{
				return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?
					.Equals("Development", StringComparison.InvariantCultureIgnoreCase) ?? false;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}

}
