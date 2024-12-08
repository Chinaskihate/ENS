using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using ENS.Contracts.NotificationConfiguration.Models;
using ENS.Contracts.NotificationConfiguration.Services;
using ENS.NotificationConfiguration.Attributes;
using System.Globalization;

namespace ENS.NotificationConfiguration.Services.Parsing;
[FileParser("csv")]
public class CsvFileParser : IFileParser
{
    public List<NotificationConfigurationDto> Parse(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
            MissingFieldFound = null,
            HeaderValidated = null
        };

        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<NotificationConfigurationDtoMap>();

        return csv.GetRecords<NotificationConfigurationDto>().ToList();
    }
}

public class NotificationConfigurationDtoMap : ClassMap<NotificationConfigurationDto>
{
    public NotificationConfigurationDtoMap()
    {
        Map(m => m.UserId).Name("user_id").Default((Guid?)null);
        Map(m => m.FirstName).Name("first_name").Default(null);
        Map(m => m.LastName).Name("last_name").Default(null);
        Map(m => m.Channels)
            .Name("channels")
            .TypeConverter<ArrayConverter<Channel>>()
            .Default((Channel[])null); // Custom converter for Channel[]
        Map(m => m.UserIdInChannels)
            .Name("user_id_in_channels")
            .TypeConverter<ArrayConverter<string>>()
            .Default((string[])null); // Custom converter for string[]
        Map(m => m.TemplateId).Name("template_id").Default(null);
        Map(m => m.TemplateMessage).Name("template_message").Default(null);
        Map(m => m.TemplateChannels)
            .Name("template_channels")
            .TypeConverter<ArrayConverter<Channel>>()
            .Default((Channel[])null); // Custom converter for Channel[]
    }
}

internal class ArrayConverter<T> : DefaultTypeConverter
{
    private const char Separator = ','; // Adjust separator if needed
    private readonly ITypeConverter? _elementConverter;

    public ArrayConverter()
    {
        // Initialize TypeConverterCache and get the appropriate converter
        var typeConverterCache = new TypeConverterCache();

        // Use EnumConverter for enums, otherwise use the default converter from TypeConverterCache
        _elementConverter = typeof(T).IsEnum
            ? (ITypeConverter)Activator.CreateInstance(typeof(EnumConverter<>).MakeGenericType(typeof(T)))
            : typeConverterCache.GetConverter(typeof(T));
    }

    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Array.Empty<T>();

        return text
            .Split(Separator, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => (T)_elementConverter.ConvertFromString(x.Trim(), row, memberMapData))
            .ToArray();
    }

    public override string ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value is not IEnumerable<T> enumerable || !enumerable.Any())
            return string.Empty;
        
        return string.Join(Separator.ToString(), enumerable.Select(x => _elementConverter.ConvertToString(x, row, memberMapData)));
    }
}

internal class EnumConverter<T> : DefaultTypeConverter where T : struct, Enum
{
    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        // Try parsing as a number first, then as a string
        if (int.TryParse(text, out var intValue))
            return Enum.ToObject(typeof(T), intValue);

        return Enum.TryParse<T>(text, true, out var result)
            ? result
            : throw new InvalidCastException($"Cannot convert '{text}' to {typeof(T).Name}.");
    }

    public override string ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value == null)
            return string.Empty;

        if (value is T enumValue)
            return Convert.ToInt32(enumValue).ToString(); // Convert enum to integer

        throw new InvalidCastException($"Cannot convert '{value}' to a string.");
    }
}
