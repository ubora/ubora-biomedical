using System.ComponentModel.DataAnnotations;
using System.IO;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Ubora.Web.Infrastructure;
using Xunit;

namespace Ubora.Web.Tests.Infrastructure
{
    public class FileSizeAttributeTests
    {
        private readonly FileSizeAttribute _fileSizeAttribute;
        private readonly ValidationContext _validationContext;
        private int MaxFileSizeBytes = 1000000;

        public FileSizeAttributeTests()
        {
            _fileSizeAttribute = new FileSizeAttribute(MaxFileSizeBytes);
            _validationContext = new ValidationContext(_fileSizeAttribute);
        }

        [Fact]
        public void FileSize_Returns_ValidationResult_When_File_Is_Empty_File()
        {
            // Act
            var result = _fileSizeAttribute.GetValidationResult(null, _validationContext);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void FileSize_Returns_ValidationResult_When_File_Is_Over_Maxiumum_Size()
        {
            var fileMock = new Mock<IFormFile>();

            using (var fileStream = new FileStream("testFile", FileMode.Create, FileAccess.ReadWrite))
            {
                fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
                fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
                fileMock.Setup(y => y.OpenReadStream()).Returns(fileStream);
                fileMock.Setup(f => f.Length).Returns(1348576);

                //Act
                var result = _fileSizeAttribute.GetValidationResult(fileMock.Object, _validationContext);

                //Assert
                result.ErrorMessage.Should().Be("Please upload a smaller file!");
            }
        }

        [Fact]
        public void FileSize_Returns_ValidationResult_Succsss_When_FileSize_Is_Under_Max_Size()
        {
            var fileMock = new Mock<IFormFile>();

            using (var fileStream = new FileStream("testFile", FileMode.Create, FileAccess.ReadWrite))
            {
                fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
                fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
                fileMock.Setup(y => y.OpenReadStream()).Returns(fileStream);
                fileMock.Setup(f => f.Length).Returns(500000);

                //Act
                var result = _fileSizeAttribute.GetValidationResult(fileMock.Object, _validationContext);

                //Assert
                Assert.Equal(ValidationResult.Success, result);
            }
        }
    }
}