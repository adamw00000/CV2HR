using CV2HR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace CV2HR.Services
{
    public interface IBlobService
    {
        Task<Uri> AddFile(IFormFile file, string blobFileName);
        string GetFileName(JobApplicationCreateViewModel viewModel);
        void ValidateFile(IFormFile formFile, ModelStateDictionary modelState);
        Task RemoveFile(string fileName);
    }
}