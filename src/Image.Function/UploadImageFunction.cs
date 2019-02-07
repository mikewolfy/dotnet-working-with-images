using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using Image.Utils;
using System.Net.Http;

namespace Image.Function
{
    public static class UploadImageFunction
    {
        public static StorageHelper helper = new StorageHelper(Environment.GetEnvironmentVariable("connectionString"));

        [FunctionName("UploadImage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req, ILogger log)
        {
            var provider = new MultipartMemoryStreamProvider();
            await req.Content.ReadAsMultipartAsync(provider);
            var file = provider.Contents.First();
            var fileInfo = file.Headers.ContentDisposition;
            var fileData = await file.ReadAsByteArrayAsync();

            var imageName = System.Guid.NewGuid().ToString();

            if (!(await helper.SaveToBlobStorage(imageName, fileData)))
            {
                return new BadRequestObjectResult("Error occurred saving file");
            }

            return (ActionResult)new OkObjectResult($"Hello, {imageName}");
        }
    }
}
