namespace ENS.NotificationConfiguration.Attributes;
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class FileParserAttribute(string fileExtension) : Attribute
{
    public string FileExtension { get; } = fileExtension;
}
