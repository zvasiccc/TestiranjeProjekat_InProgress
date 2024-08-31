namespace TestiranjeProjekat.Exceptions
{
    public class ExistingOrganizatorException : Exception
    {
        public int StatusCode { get; }
        public ExistingOrganizatorException() : base("Organizator already exists")
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
        public ExistingOrganizatorException(string msg) : base(msg)
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
    }
}