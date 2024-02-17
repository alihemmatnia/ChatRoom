namespace ChatRoom.Core.Domain.Entities
{
	public class Group
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public List<User> Members { get; private set; } = new();
		public Guid OwnerId { get; private set; }
		public DateTime CreatedDate { get; private set; }

		public Group(string name, Guid ownerId)
		{
			Name = name;
			OwnerId = ownerId;
			CreatedDate = DateTime.Now;
		}

		public void AddMember(User member)
		{
			Members.Add(member);
		}

		public void RemoveMember(User member)
		{
			Members.Remove(member);
		}
	}
}
