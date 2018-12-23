using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CV_2_HR.Models
{
    public class JobOffer: IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }

        public virtual Company Company { get; set; }
        [Required(ErrorMessage = "The Company field is required.")]
        public int CompanyId { get; set; }
        
        [Display(Name = "Salary from")]
        public decimal? SalaryFrom { get; set; }
        
        [Display(Name = "Salary to")]
        public decimal? SalaryTo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyy HH:mm:ss}")]
        public DateTime Created { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [MinLength(100, ErrorMessage = "Description should contain at least 100 characters")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyy}")]
        [Display(Name = "Valid until")]
        public DateTime? ValidUntil { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ICollection<JobApplication> JobApplications { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SalaryFrom > SalaryTo)
            {
                yield return new ValidationResult("Salary to must be equal or greater than salary from.", new[] { "SalaryTo" });
            }
            if (SalaryFrom <= 0)
            {
                yield return new ValidationResult("Salary must be greater than 0.", new[] { "SalaryFrom" });
            }
            if (SalaryTo <= 0)
            {
                yield return new ValidationResult("Salary must be greater than 0.", new[] { "SalaryTo" });
            }
            if (ValidUntil < DateTime.Now)
            {
                yield return new ValidationResult("Valid until cannot be a past date", new[] { "ValidUntil" });
            }

            var result = WebUtility.HtmlDecode(Description);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(result);
            var text = htmlDoc.DocumentNode.InnerText;

            if (text.Length < 100)
            {
                yield return new ValidationResult("Description should contain at least 100 characters", new[] { "Description" });
            }
        }
    }
}
