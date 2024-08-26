namespace TestiranjeProjekat.Exceptions
{
    public class NonExistingRegistrationException : Exception
    {
        public NonExistingRegistrationException() : base("registration does not exists")
        {

        }
        public NonExistingRegistrationException(string msg) : base(msg)
        {

        }
    }
}