namespace TestiranjeProjekat.Exceptions
{
    public class NonExistingTournamentException : Exception
    {
        public NonExistingTournamentException() : base("non existing tournament")
        {

        }
        public NonExistingTournamentException(string msg) : base(msg)
        {

        }
    }
}