namespace TestiranjeProjekat.Exceptions
{
    public class ExistingPlayerException : Exception
    {
        public ExistingPlayerException() : base("Player already exists")
        {

        }
        public ExistingPlayerException(string msg) : base(msg)
        {

        }
    }
}