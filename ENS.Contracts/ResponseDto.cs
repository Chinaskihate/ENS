namespace ENS.Contracts;
public class ResponseDto
{
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; }
    public string[] Warnings { get; set; }
}
