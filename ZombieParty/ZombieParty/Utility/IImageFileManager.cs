namespace ZombieParty.Utility
{
    public interface IImageFileManager
    {
        string? UploadImage(IFormCollection form, bool edit, string? oldImagePath = null);
    }
}