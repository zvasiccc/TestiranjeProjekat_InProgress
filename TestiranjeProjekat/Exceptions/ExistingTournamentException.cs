namespace TestiranjeProjekat.Exceptions
{
    public class
    ExistingTournamentException : Exception
    {
        public int StatusCode { get; }
        ExistingTournamentException() : base("tournament already exists")
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
        public
        ExistingTournamentException(string msg) : base(msg)
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
    }
}