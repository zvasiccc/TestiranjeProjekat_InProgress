namespace Backend.Exceptions
{
    public class EmptyTournamentListException : Exception
    {
        public int StatusCode { get; }
        public EmptyTournamentListException(string msg) : base(msg)
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
        public EmptyTournamentListException() : base("tournament list is empty")
        {
            StatusCode = StatusCodes.Status404NotFound;

        }
    }
}