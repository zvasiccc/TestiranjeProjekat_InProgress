namespace TestiranjeProjekat.Exceptions
{
    public class NonExistingTeamLeaderException : Exception
    {
        public int StatusCode { get; }
        public NonExistingTeamLeaderException() : base("Team leader does not exist")
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
        public NonExistingTeamLeaderException(string msg) : base(msg)
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
    }
}