using ChatRoom.Application.Services.Interfaces;
using ChatRoom.Core.Domain.Entities;
using ChatRoom.Framework.Dtos;
using ChatRoom.Framework.Helpers;
using ChatRoom.Infrastracture.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ChatRoom.Application.Services
{
	public class UserApplicationService : IUserApplicationService
	{
		private readonly IUserRepository _userRepository;
		private readonly ILogger<UserApplicationService> _logger;

		public UserApplicationService(IUserRepository userRepository, ILogger<UserApplicationService> logger)
		{
			_userRepository = userRepository;
			_logger = logger;
		}

		public async Task<ApiResponse<string>> AddUser(UserDto userDto, CancellationToken cancellationToken)
		{
			var checkUser = await _userRepository.GetOneAsync(x => x.UserName == userDto.UserName);
			if (checkUser != null)
				return new ApiResponse<string> { Success = false, Message = "نام کاربری تکراری میباشد" };

			var user = new User(userDto.UserName);

			var password = PasswordHash.HashPasword(userDto.Password, out var salt);
			user.SetPassword(password, salt);

			_userRepository.Add(user);
			await _userRepository.SaveChangesAsync();

			_logger.LogInformation("user {0} has been created", userDto.UserName);

			return new ApiResponse<string>()
			{
				Message = "اکانت شما با موفقیت ایجاد شد",
				Success = true,
			};
		}
	}
}
