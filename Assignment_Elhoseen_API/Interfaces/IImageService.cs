namespace EcommerceAPI.Interfaces
{
    public interface IImageService
    {
        Task<string?> UploadImageAsync(IFormFile file, string folderName);
    }
}
