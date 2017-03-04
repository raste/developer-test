using System.Collections.Generic;

using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Offers.ViewModels
{
    public class MyOffersViewModel
    {
       public bool HasMadeOffers { get { return OffersStates != null 
                                             && OffersStates.Count > 0; } }
       public List<PropertyOfferState> OffersStates { get; set; }
    }

    public class PropertyOfferState
    {
        public string PropertyType { get; set; }
        public string PropertyStreetName { get; set; }
        public int OfferAmount { get; set; }
        public OfferStatus OfferStatus { get; set; }
    }
}