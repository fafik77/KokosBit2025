namespace kokos.Api.Exceptions
{
	public class UserAlreadyExistsException : UserException
	{
		public UserAlreadyExistsException(string msg) : base(msg)
		{
		}
	}
}
