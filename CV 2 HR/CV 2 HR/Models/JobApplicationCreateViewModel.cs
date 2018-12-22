using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CV_2_HR.Models
{
    public class JobApplicationCreateViewModel: JobApplication
    {
        [Required]
        [Display(Name = "CV file (.pdf)")]
        public IFormFile CvFile { get; set; }
    }
}
