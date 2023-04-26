namespace Models.File;

public class InvalidFileException : Exception
{
    public InvalidFileException() { }

    public InvalidFileException(string name)
        : base(String.Format("{0}", name))
    {

    }
}
