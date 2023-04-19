namespace Models.Database;

public class InvalidDbException : Exception
{
    public InvalidDbException() { }

    public InvalidDbException(string name)
        : base(String.Format("{0}", name))
    {

    }
}
