using CV_2_HR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CV_2_HR.Services
{
    public class BlobService: IBlobService
    {
        public async Task<bool> AddFile(IFormFile file, string blobFileName)
        {
            //byte[] fileContent;
            //await file.CopyToAsync(fileContent);
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=jobofferblobaw;AccountKey=icysqqg5VVrK0Wu24B0GL2tx4izPQFFthl+mdc9mGOfanvdaZo7aVFbbCEg7pytG+UmLjKeZrH/ZmYEZniFZFw==;EndpointSuffix=core.windows.net";
            try
            {
                if (CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
                    CloudBlobContainer container = cloudBlobClient.GetContainerReference("applications");
                    //await container.CreateIfNotExistsAsync();

                    // Get the reference to the block blob from the container
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobFileName);

                    // Upload the file
                    using (var stream = file.OpenReadStream())
                    {
                        await blockBlob.UploadFromStreamAsync(stream);
                    }
                    //await blockBlob.UploadFromByteArrayAsync(file, 0, file.Length);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetFileName(JobApplicationCreateViewModel viewModel)
        {
            return "cv_" + viewModel.Id + "_" + DateTime.Now.Ticks + ".pdf";
        }
        
        public void ValidateFile(IFormFile formFile, ModelStateDictionary modelState)
        {
            var fieldDisplayName = string.Empty;
            
            MemberInfo property = typeof(JobApplicationCreateViewModel)
                .GetProperty(formFile.Name.Substring(formFile.Name.IndexOf(".") + 1));

            if (property != null)
            {
                if (property.GetCustomAttribute(typeof(DisplayAttribute)) is DisplayAttribute displayAttribute)
                {
                    fieldDisplayName = $"{displayAttribute.Name} ";
                }
            }
            
            var fileName = WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));

            if (formFile.ContentType.ToLower() != "application/pdf")
            {
                modelState.AddModelError(formFile.Name,
                    $"The {fieldDisplayName} ({fileName}) must be a pdf file.");
            }
            
            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name,
                    $"The {fieldDisplayName}file ({fileName}) is empty.");
            }
            else if (formFile.Length > 1048576)
            {
                modelState.AddModelError(formFile.Name,
                    $"The {fieldDisplayName}file ({fileName}) exceeds 1 MB.");
            }
        }
    }
}
