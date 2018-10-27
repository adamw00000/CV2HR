using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_2_HR.Models
{
    public class JobOfferCreateViewModel : JobOffer
    {
        public IEnumerable<Company> Companies { get; set; }
    }
}
