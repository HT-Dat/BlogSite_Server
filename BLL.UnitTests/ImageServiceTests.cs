using System.IO;
using System.Threading.Tasks;
using BLL.Services;
using BLL.Utilities;
using Google.Cloud.Storage.V1;
using Moq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace BLL.UnitTests;

public class ImageServiceTests
{
    private readonly ImageService _imageServiceUnderTest;
    private readonly Mock<StorageClient> _storageClientMock = new Mock<StorageClient>();
    private readonly Mock<ITimeHelper> _timeHelperMock = new Mock<ITimeHelper>();

    public ImageServiceTests()
    {
        _imageServiceUnderTest = new ImageService(_storageClientMock.Object, _timeHelperMock.Object);
    }

    [Fact]
    public async Task ResizeImage_Should_ResizeImageCorrectly()
    {
        // Arrange
        var maxWidth = 300;
        var maxHeight = 100;
        var testImageWidth = 300;
        var testImageHeight = 200;
        double testImageRatio = (double)testImageWidth / testImageHeight;
        var testImage = new Image<Rgba32>(testImageWidth, testImageHeight);

        MemoryStream testImageStream = new MemoryStream();
        await testImage.SaveAsJpegAsync(testImageStream);
        testImageStream.Position = 0;

        //Act
        var actualStream = await _imageServiceUnderTest.ResizeImage(testImageStream, maxWidth, maxHeight);
        actualStream.Position = 0;
        var actualImage = await Image.LoadAsync(actualStream);
        double actualImageRatio = (double)actualImage.Width / actualImage.Height;
        
        // Assert
        Assert.True(actualImage.Width <= maxWidth);
        Assert.True(actualImage.Height <= maxHeight);
        Assert.Equal(testImageRatio, actualImageRatio, 2);
    }
}