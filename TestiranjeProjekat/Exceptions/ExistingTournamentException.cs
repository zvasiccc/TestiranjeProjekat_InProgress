namespace TestiranjeProjekat.Exceptions
{
    public class
    ExistingTournamentException : Exception
    {
        public
        ExistingTournamentException() : base("tournament already exists")
        {

        }
        public
        ExistingTournamentException(string msg) : base(msg)
        {

        }
    }
}