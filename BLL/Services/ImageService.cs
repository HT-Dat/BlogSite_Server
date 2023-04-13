using BLL.Services.IServices;
using BLL.Utilities;
using Google.Cloud.Storage.V1;

namespace BLL.Services;

public class ImageService : IImageService
{
    private readonly StorageClient _storageClient;
    private readonly ITimeHelper _timeHelper;
    public ImageService(StorageClient storageClient, ITimeHelper timeHelper)
    {
        _storageClient = storageClient;
        _timeHelper = timeHelper;
    }


    public async Task<MemoryStream> ResizeImage(MemoryStream stream, int maxWidth, int maxHeight)
    {
        Image image = await Image.LoadAsync(stream);
        var resizeOptions = new ResizeOptions()
        {
            Size = new Size(maxWidth, maxHeight),
            Mode = ResizeMode.Max
        };
        image.Mutate(x => x.Resize(resizeOptions));
        MemoryStream outputStream = new MemoryStream();
        await image.SaveAsync(outputStream, image.Metadata.DecodedImageFormat);
        return outputStream;
    }

    public async Task<string> UploadImageFireStore(MemoryStream stream, string upFileName, string contentType)
    {
        
        var fileName = Path.GetFileNameWithoutExtension(upFileName) +
                       _timeHelper.CurrentNumericDate() + Path.GetExtension(upFileName);
        
        string bucketName = "hotiendat-blog.appspot.com";
        var bucket = _storageClient.GetBucket(bucketName);
        var obj = new Google.Apis.Storage.v1.Data.Object
        {
            Bucket = bucketName,
            Name = $"post_img/{fileName}",
            ContentType = contentType
        };
        await _storageClient.UploadObjectAsync(obj, await ResizeImage(stream, 600, 600));
        return $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/{Uri.EscapeDataString(obj.Name)}?alt=media";
    }
}