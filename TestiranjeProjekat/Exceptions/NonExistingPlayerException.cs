namespace TestiranjeProjekat.Exceptions
{
    public class NonExistingPlayerException : Exception
    {
        public int StatusCode { get; }
        public NonExistingPlayerException() : base("This player does not exist")
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
        public NonExistingPlayerException(string msg) : base(msg)
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
    }
}