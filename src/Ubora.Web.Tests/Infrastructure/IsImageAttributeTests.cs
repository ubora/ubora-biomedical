using System.ComponentModel.DataAnnotations;
using System.IO;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Ubora.Web.Infrastructure;
using Xunit;

namespace Ubora.Web.Tests.Infrastructure
{
    public class IsImageAttributeTests
    {
        private readonly IsImageAttribute _isImageAttribute;
        private readonly ValidationContext _validationContext;

        public IsImageAttributeTests()
        {
            _isImageAttribute = new IsImageAttribute();
            _validationContext = new ValidationContext(_isImageAttribute);
        }

        [Fact]
        public void IsImage_Returns_ValidationResult_When_The_Image_Is_Empty_File()
        {
            //Act
            var result = _isImageAttribute.GetValidationResult(null, _validationContext);

            //Assert
            result.ErrorMessage.Should().Be("Please select an image to upload first!");
        }

        [Fact]
        public void IsImage_Returns_ValidationResult_When_The_Image_Is_Not_An_Image_Mime_Type()
        {
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(f => f.ContentType).Returns("video/3gpp2");

            //Act
            var result = _isImageAttribute.GetValidationResult(fileMock.Object, _validationContext);

            //Assert
            result.ErrorMessage.Should().Be("This is not an image file!");
        }

        [Fact]
        public void IsImage_Returns_ValidationResult_When_The_Image_Is_Not_An_Image_Extension()
        {
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.exe");

            //Act
            var result = _isImageAttribute.GetValidationResult(fileMock.Object, _validationContext);

            //Assert
            result.ErrorMessage.Should().Be("This is not an image file extension");
        }

        [Fact]
        public void IsImage_Returns_ValidationResult_When_The_Image_File_Is_Not_Readable()
        {
            var fileMock = new Mock<IFormFile>();

            using (var fileStream = new FileStream("onlyWriteAccessFile", FileMode.Create, FileAccess.Write))
            {
                fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
                fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
                fileMock.Setup(f => f.OpenReadStream()).Returns(fileStream);

                //Act
                var result = _isImageAttribute.GetValidationResult(fileMock.Object, _validationContext);

                //Assert
                result.ErrorMessage.Should().Be("The image file is not readable");
            }
        }

        [Fact]
        public void IsImage_Returns_ValidationResult_When_The_Image_Is_Under_Minimum_Size()
        {
            var fileMock = new Mock<IFormFile>();

            using (var fileStream = new FileStream("testFile", FileMode.Create, FileAccess.ReadWrite))
            {
                fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
                fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
                fileMock.Setup(y => y.OpenReadStream()).Returns(fileStream);
                fileMock.Setup(f => f.Length).Returns(500);

                //Act
                var result = _isImageAttribute.GetValidationResult(fileMock.Object, _validationContext);

                //Assert
                result.ErrorMessage.Should().Be("This is not an image file!");
            }
        }

        [Fact]
        public void IsImage_Returns_ValidationResult_When_Is_Empty_FormFile()
        {
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");

            //Act
            var result = _isImageAttribute.GetValidationResult(fileMock.Object, _validationContext);

            //Assert
            result.ErrorMessage.Should().Be("This is not an image file!");
        }
    }
}
