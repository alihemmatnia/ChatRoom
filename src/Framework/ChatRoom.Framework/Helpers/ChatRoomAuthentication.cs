using ChatRoom.Framework.Configuration;
using ChatRoom.Framework.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatRoom.Framework.Helpers
{
	public static class ChatRoomAuthentication
	{
		public static SymmetricSecurityKey AuthSigningKey;
		public static SymmetricSecurityKey AuthDecryptionKey;
		public static AuthConfiguration Configuration;

		public static AuthToken GenerateToken(IEnumerable<Claim> claims)
		{
			var token = new JwtSecurityToken(
					issuer: Configuration.Issuer,
					audience: Configuration.Audience,
					expires: DateTime.Now.AddMinutes(Configuration.ExpiresMinute),
					claims: claims,
					signingCredentials: new SigningCredentials(AuthSigningKey, SecurityAlgorithms.HmacSha256)
			);

			return new AuthToken()
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				Expires = token.ValidTo
			};
		}

		static async ValueTask<string> GenerateUniqueToken(Func<string, Task<bool>> tokenIsUnique = null)
		{
			var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).Replace(".", "%");
			if (tokenIsUnique != null && !await tokenIsUnique(token))
				return await GenerateUniqueToken(tokenIsUnique);

			return token;
		}

		public static IEnumerable<Claim> ReadClaims(HttpRequestMessage request)
		{
			var token = GetToken(request);
			var handler = new JwtSecurityTokenHandler();
			var jwtSecurityToken = handler.ReadJwtToken(token);
			var validations = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = AuthSigningKey,
				TokenDecryptionKey = AuthDecryptionKey,
				ValidateIssuer = Configuration.ValidateIssuer,
				ValidateAudience = Configuration.ValidateAudience
			};

			var claimsPrincipal = handler.ValidateToken(token, validations, out var tokenSecure);
			if (claimsPrincipal.Identity == null || !claimsPrincipal.Identity.IsAuthenticated)
				throw new UnauthorizedAccessException("claims authorization failed - response:Unauthorized(401)");

			return jwtSecurityToken.Claims;
		}

		public static string GetToken(HttpRequestMessage request) => request.Headers.Authorization.Parameter;

		public static void Configure()
		{
			AuthDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.DecryptionKey));
			AuthSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.SigningKey));
		}
	}

}
