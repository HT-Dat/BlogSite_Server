namespace BLL.Services.IServices;

public interface IImageService
{
    Task<MemoryStream> ResizeImage(MemoryStream stream, int maxWidth, int maxHeight);
    Task<string> UploadImageFireStore(MemoryStream stream, string upFileName, string contentType);
}