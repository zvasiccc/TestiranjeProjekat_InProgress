namespace TestiranjeProjekat.Exceptions
{
    public class ExistingPlayerException : Exception
    {
        public int StatusCode { get; }
        public ExistingPlayerException() : base("Player already exists")
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
        public ExistingPlayerException(string msg) : base(msg)
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
    }
} 