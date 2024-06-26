﻿using ChatRoom.Framework.Dtos;

namespace ChatRoom.Application.Services.Interfaces
{
	public interface IUserApplicationService
	{
		Task<ApiResponse<string>> AddUser(UserDto userDto, CancellationToken cancellationToken);
		Task<ApiResponse<string>> Login(UserDto userDto, CancellationToken cancellationToken);
	}
}
