namespace ChatRoom.Core.Domain.Entities
{
	public class User
	{
		public Guid Id { get; private set; }
		public string UserName { get; private set; }
		public string PasswordHash { get; private set; }
		public byte[] PasswordSalt { get; private set; }
		public DateTime CreatedDate { get; private set; }
		public DateTime LastLoginDate { get; private set; }
        public string? ProfilePicture { get; private set; }
        public List<Group> Groups{ get; private set; }

        public User(string username)
		{
			if(string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException("username");

			UserName = username;
			CreatedDate = DateTime.Now;
			LastLoginDate = DateTime.Now;
		}

		public void SetPassword(string passwordHash, byte[] passwordSalt)
		{
			PasswordHash = passwordHash;
			PasswordSalt = passwordSalt;
		}

		public void SetProfile(string path)
		{
			ProfilePicture = path;
		}

		public void ChangeLastLoginTime()
		{
			LastLoginDate = DateTime.Now;
		}
	}
}
