namespace TestiranjeProjekat.Exceptions
{
    public class NonExistingIdException : Exception
    {
        public int StatusCode { get; }
        public NonExistingIdException() : base("This ID don't exists")
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
        public NonExistingIdException(string msg) : base(msg)
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
    }
}