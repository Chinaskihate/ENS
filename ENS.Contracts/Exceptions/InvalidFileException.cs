using System.ComponentModel.DataAnnotations;

namespace ENS.Contracts.Exceptions;
public class InvalidFileException(string message, string fileName) : ValidationException(message)
{
    public string FileName { get; set; } = fileName;
}
