namespace kokos.Api.Exceptions
{
	public class InvalidUserException : UserException
	{
		public InvalidUserException(string msg) : base(msg)
		{
		}
	}
}
