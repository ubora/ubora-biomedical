using System.IO;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Ubora.Web._Features.Users.Manage;
using Xunit;

namespace Ubora.Web.Tests._Features.Users.Manage
{
    public class ManageValidatorTests
    {
        private readonly ManageValidator _manageValidator;

        public ManageValidatorTests()
        {
            _manageValidator = new ManageValidator();
        }

        [Fact]
        public void IsImage_Returns_ValidationResult_When_This_Is_Not_An_Image_Mime_Type()
        {
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(f => f.ContentType).Returns("video/3gpp2");

            //Act
            var result = _manageValidator.IsImage(fileMock.Object);

            //Assert
            result.IsFailure.Should().Be(true);
            result.Errors.Keys.Should().Contain("IsImage");
            result.OnlyMessages.Should().Contain("This is not an image file");
        }

        [Fact]
        public void IsImage_Returns_ValidationResult_When_This_Is_Not_An_Image_Extension()
        {
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.exe");

            //Act
            var result = _manageValidator.IsImage(fileMock.Object);

            //Assert
            result.IsFailure.Should().Be(true);
            result.Errors.Keys.Should().Contain("IsImage");
            result.OnlyMessages.Should().Contain("This is not an image file extension");
        }

        [Fact]
        public void IsImage_Returns_ValidationResult_When_The_Image_File_Is_Not_Readable()
        {
            var fileMock = new Mock<IFormFile>();

            var ms = new FileStream("onlyWriteAccessFile", FileMode.Create, FileAccess.Write);

            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            
            //Act
            var result = _manageValidator.IsImage(fileMock.Object);

            //Assert
            result.IsFailure.Should().Be(true);
            result.Errors.Keys.Should().Contain("IsImage");
            result.OnlyMessages.Should().Contain("The image file is not readable");
        }

        [Fact]
        public void IsImage_Returns_ValidationResult_When_The_Image_Is_Under_Minimum_Size()
        {
            var fileMock = new Mock<IFormFile>();

            var ms = new FileStream("testFile", FileMode.Create, FileAccess.ReadWrite);

            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            fileMock.Setup(y => y.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.Length).Returns(500);

            //Act
            var result = _manageValidator.IsImage(fileMock.Object);

            //Assert
            result.IsFailure.Should().Be(true);
            result.Errors.Keys.Should().Contain("IsImage");
            result.OnlyMessages.Should().Contain("This is not an image file");

            ms.Dispose();
        }

        [Fact]
        public void IsImage_Returns_ValidationResult_When_The_Image_Is_Over_Maximum_Size()
        {
            var fileMock = new Mock<IFormFile>();

            var ms = new FileStream("testFile", FileMode.Create, FileAccess.ReadWrite);

            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            fileMock.Setup(y => y.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.Length).Returns(1348576);

            //Act
            var result = _manageValidator.IsImage(fileMock.Object);

            //Assert
            result.IsFailure.Should().Be(true);
            result.Errors.Keys.Should().Contain("IsImage");
            result.OnlyMessages.Should().Contain("The limit for profile images is 1 MB");

            ms.Dispose();
        }

        [Fact]
        public void IsImage_Returns_ValidationResult_When_Is_Empty_FormFile()
        {
            var fileMock = new Mock<IFormFile>();

            var ms = new FileStream("testFile", FileMode.Create, FileAccess.ReadWrite);
            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");


            //Act
            var result = _manageValidator.IsImage(fileMock.Object);

            //Assert
            result.IsFailure.Should().Be(true);
            result.Errors.Keys.Should().Contain("IsImage");
            result.OnlyMessages.Should().Contain("This is not an image file");

            ms.Dispose();
        }

    }
}
