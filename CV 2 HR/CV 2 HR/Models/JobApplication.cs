using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CV_2_HR.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }
        [Display(Name = "Contact agreement")]
        public bool ContactAgreement { get; set; }
        [Url]
        [Display(Name = "CV url")]
        public string CvUrl { get; set; }
        
        public virtual JobOffer Offer { get; set; }
    }
}
