using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Models.Trips
{
    public class TripsListingViewModel
    {
        public IEnumerable<TripsAllViewModel> Trips { get; set; }
    }
}
