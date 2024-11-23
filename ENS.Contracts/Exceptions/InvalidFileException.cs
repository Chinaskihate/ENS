namespace ENS.Contracts.Exceptions;
public class InvalidFileException(string message, string fileName) : Exception(message)
{
    public string FileName { get; set; } = fileName;
}
