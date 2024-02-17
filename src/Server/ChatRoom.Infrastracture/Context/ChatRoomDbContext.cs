using ChatRoom.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.Infrastracture.Context
{
	public class ChatRoomDbContext : DbContext
	{
		public ChatRoomDbContext(DbContextOptions<ChatRoomDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Group> Groups { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasMany(x => x.Groups)
				.WithMany(x => x.Members);
		}
	}
}
