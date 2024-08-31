namespace TestiranjeProjekat.Exceptions
{
    public class NonExistingOrganizatorException : Exception
    {
        public int StatusCode { get; }
        public NonExistingOrganizatorException() : base("This organizator does not exist")
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
        public NonExistingOrganizatorException(string msg) : base(msg)
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
    }
}