namespace ChatRoom.Framework.Dtos
{
	public class ApiResponse<T>
	{
		public int Code { get; set; }
		public T Message { get; set; }
		public bool Success { get; set; }
	}
}
