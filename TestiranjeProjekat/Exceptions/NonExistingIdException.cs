namespace TestiranjeProjekat.Exceptions
{
    public class NonExistingOrganizatorException : Exception
    {
        public NonExistingOrganizatorException() : base("This ID don't exists")
        {

        }
        public NonExistingOrganizatorException(string msg) : base(msg)
        {

        }
    }
}