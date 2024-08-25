namespace TestiranjeProjekat.Exceptions
{
    public class EmptyFieldException : Exception
    {
        public EmptyFieldException() : base("Some of the fields are empty")
        {

        }
        public EmptyFieldException(string msg) : base(msg)
        {

        }
    }
}