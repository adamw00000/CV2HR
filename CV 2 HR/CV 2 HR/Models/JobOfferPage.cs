using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV2HR.Models
{
    public class JobOfferPage
    {
        public IEnumerable<JobOffer> JobOffers { get; set; }
        public int Pages { get; set; }
    }
}
