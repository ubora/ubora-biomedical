using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.IO;

namespace Ubora.Web.Tests
{
    public static class TestFormFiles
    {
        public static IFormFile CreateRandom()
        {
            return Mock.Of<IFormFile>(f => 
                f.FileName == $"/test/{Guid.NewGuid()}.jpg"
                && f.OpenReadStream() == new MemoryStream());
        }
    }
}
