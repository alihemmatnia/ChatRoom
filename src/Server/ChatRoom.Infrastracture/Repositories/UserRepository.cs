using ChatRoom.Core.Domain.Entities;
using ChatRoom.Infrastracture.Context;
using ChatRoom.Infrastracture.Repositories.Interfaces;

namespace ChatRoom.Infrastracture.Repositories
{
	public class UserRepository : EntityRepository<User, ChatRoomDbContext>, IUserRepository
	{
		public UserRepository(IUnitOfWork<ChatRoomDbContext> context) : base(context)
		{
		}

		public override Task<User> CreateOrUpdateAsync(User entity)
		{
			throw new NotImplementedException();
		}
	}
}
