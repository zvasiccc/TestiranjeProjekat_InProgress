namespace TestiranjeProjekat.Exceptions
{
    public class NonExistingTournamentException : Exception
    {
        public int StatusCode { get; }
        public NonExistingTournamentException() : base("non existing tournament")
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
        public NonExistingTournamentException(string msg) : base(msg)
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
    }
}