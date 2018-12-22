using CV_2_HR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace CV_2_HR.Services
{
    public interface IBlobService
    {
        Task<bool> AddFile(IFormFile file, string blobFileName);
        string GetFileName(JobApplicationCreateViewModel viewModel);
        void ValidateFile(IFormFile formFile, ModelStateDictionary modelState);
    }
}