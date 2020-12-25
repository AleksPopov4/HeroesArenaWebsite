using Microsoft.WindowsAzure.Storage.Blob;

namespace HeroesArenaWebsite.Services.Data
{
    public interface IUploadService
    {
            CloudBlobContainer GetBlobContainer(string connectionString);
    }
}
