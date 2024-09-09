namespace TestiranjeProjekat.Exceptions
{
    public class FullTournamentCapacityException : Exception
    {
        public int StatusCode { get; }
        public FullTournamentCapacityException() : base("tournament teams capacity is full ")
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
        public FullTournamentCapacityException(string msg) : base(msg)
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
    }
}