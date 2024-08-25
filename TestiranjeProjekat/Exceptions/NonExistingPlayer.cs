namespace TestiranjeProjekat.Exceptions
{
    public class NonExistingPlayerException : Exception
    {
        public NonExistingPlayerException() : base("This ID don't exists")
        {

        }
        public NonExistingPlayerException(string msg) : base(msg)
        {

        }
    }
}