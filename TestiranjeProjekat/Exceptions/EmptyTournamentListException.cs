namespace Backend.Exceptions
{
    public class EmptyTournamentListException : Exception
    {
        public EmptyTournamentListException(string msg) : base(msg)
        {

        }
        public EmptyTournamentListException() : base("tournament list is empty")
        {

        }
    }
}