namespace TestiranjeProjekat.Exceptions
{
    public class ExistingOrganizatorException : Exception
    {
        public ExistingOrganizatorException() : base("Organizator already exists")
        {

        }
        public ExistingOrganizatorException(string msg) : base(msg)
        {

        }
    }
}