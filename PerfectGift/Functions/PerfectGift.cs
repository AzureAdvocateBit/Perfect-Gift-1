using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PerfectGift
{
    public class PerfectGiftFunction
    {
        static readonly IReadOnlyCollection<string> _requiredPhotoTags = new List<string> { "box", "gift wrapping", "ribbon", "present" };

        readonly ILogger _logger;
        readonly ComputerVisionService _computerVisionService;

        public PerfectGiftFunction(ComputerVisionService computerVisionService, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PerfectGiftFunction>();

            _computerVisionService = computerVisionService;
            computerVisionService.InvalidPhotoSubmitted += HandleInvalidPhotoSubmitted;
        }

        [FunctionName(nameof(PerfectGiftFunction))]
        public async Task Run([BlobTrigger("gifts")]CloudBlockBlob giftPhotoBlob)
        {
            _logger.LogInformation("Blob Storage Function Tiggered");

            _logger.LogInformation("Creating Stream");


            if (isPhotoValid)
            {
                _logger.LogInformation("Perfect Gift Confirmed");
            using var giftPhotoStream = await giftPhotoBlob.OpenReadAsync().ConfigureAwait(false);
            }
            else
            {
                _logger.LogInformation($"Photo {giftPhotoBlob.Name} is not a perfect gift");

                _logger.LogInformation($"Deleting {giftPhotoBlob.Name} from container");

                await giftPhotoBlob.DeleteAsync().ConfigureAwait(false);

                _logger.LogInformation($"Deleted {giftPhotoBlob.Name}");
            }
        }

        void HandleInvalidPhotoSubmitted(object? sender, InvalidPhotoEventArgs e) => _logger.LogInformation(e.ToString());
    }
}
