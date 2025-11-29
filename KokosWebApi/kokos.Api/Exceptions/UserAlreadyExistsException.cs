namespace kokos.Api.Exceptions
{
	public class UserAlreadyExistsException : Exception
	{
		public UserAlreadyExistsException(string msg) : base(msg)
		{
		}
	}
}
