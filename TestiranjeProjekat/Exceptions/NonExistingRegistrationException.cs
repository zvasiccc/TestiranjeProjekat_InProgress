namespace TestiranjeProjekat.Exceptions
{
    public class NonExistingRegistrationException : Exception
    {
        public int StatusCode { get; }
        public NonExistingRegistrationException() : base("registration does not exists")
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
        public NonExistingRegistrationException(string msg) : base(msg)
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
    }
}