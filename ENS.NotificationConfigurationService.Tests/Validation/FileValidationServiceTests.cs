using ENS.Contracts.Exceptions;
using ENS.NotificationConfiguration.Services.Validation;
using ENS.Resources.Errors;
using FluentAssertions;
using Moq;

namespace ENS.NotificationConfigurationService.Tests.Validation;

[TestFixture]
public class FileValidationServiceTests
{
    private Mock<IFormFile> _mockFile;
    private FileValidationSettings _settings;
    private FileValidationService _service;

    [SetUp]
    public void SetUp()
    {
        _mockFile = new Mock<IFormFile>();

        // Configure settings
        _settings = new FileValidationSettings
        {
            MaxSizeInBytes = 10 * 1024, // 10 KB
            AllowedExtensions = new[] { "csv", "xlsx" }
        };

        _service = new FileValidationService(_settings);
    }

    [Test]
    public void Validate_EmptyFile_ThrowsInvalidFileException()
    {
        // Arrange
        _mockFile.Setup(f => f.Length).Returns(0);

        // Act
        Action act = () => _service.Validate(_mockFile.Object);

        // Assert
        act.Should()
            .Throw<InvalidFileException>()
            .WithMessage(Errors.EmptyFile)
            .And.FileName.Should().BeNullOrEmpty(); // Additional check for file name
    }

    [Test]
    public void Validate_FileTooBig_ThrowsInvalidFileException()
    {
        // Arrange
        _mockFile.Setup(f => f.Length).Returns(_settings.MaxSizeInBytes + 1);

        // Act
        Action act = () => _service.Validate(_mockFile.Object);

        // Assert
        act.Should()
            .Throw<InvalidFileException>()
            .WithMessage(Errors.FileTooBig);
    }

    [Test]
    public void Validate_UnsupportedExtension_ThrowsInvalidFileException()
    {
        // Arrange
        _mockFile.Setup(f => f.FileName).Returns("test.txt");
        _mockFile.Setup(f => f.Length).Returns(1024); // Within size limit

        // Act
        Action act = () => _service.Validate(_mockFile.Object);

        // Assert
        act.Should()
            .Throw<InvalidFileException>()
            .WithMessage(Errors.UnsupportedExtension)
            .And.FileName.Should().Be("test.txt"); // Ensure the file name is passed correctly
    }

    [Test]
    public void Validate_SupportedFile_PassesValidation()
    {
        // Arrange
        _mockFile.Setup(f => f.FileName).Returns("test.csv");
        _mockFile.Setup(f => f.Length).Returns(1024); // Within size limit

        // Act
        Action act = () => _service.Validate(_mockFile.Object);

        // Assert
        act.Should().NotThrow();
    }

    [Test]
    public void Validate_FileWithoutExtension_ThrowsInvalidFileException()
    {
        // Arrange
        _mockFile.Setup(f => f.FileName).Returns("file_without_extension");
        _mockFile.Setup(f => f.Length).Returns(1024); // Within size limit

        // Act
        Action act = () => _service.Validate(_mockFile.Object);

        // Assert
        act.Should()
            .Throw<InvalidFileException>()
            .WithMessage(Errors.UnsupportedExtension)
            .And.FileName.Should().Be("file_without_extension");
    }
}
