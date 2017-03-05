using System.Linq;

using OrangeBricks.Web.Controllers.Offers.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Offers.Builders
{
    public class MyOffersViewModelBuilder
    {
        private readonly IOrangeBricksContext _context;

        public MyOffersViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns model that contains all appointment requests made by
        /// the user. The appointments are sorted by descending based on
        /// their creation date.
        /// </summary>
        public MyOffersViewModel Build(string userID)
        {
            var offersStates = _context.Properties
                                       .Where(p => p.Offers.Any(o => o.BuyerUserId == userID))
                                       .Select(p => new
                                       {
                                           p.StreetName,
                                           p.PropertyType,
                                           Offers = p.Offers.Where(o => o.BuyerUserId == userID)
                                       })
                                       .ToList()
                                       .SelectMany(p => p.Offers.Select(o => new
                                                 {
                                                     o.CreatedAt,
                                                     OfferState = new PropertyOfferState()
                                                     {
                                                         OfferAmount = o.Amount,
                                                         PropertyType = p.PropertyType,
                                                         OfferStatus = o.Status,
                                                         PropertyStreetName = p.StreetName
                                                     }
                                                  }))
                                        .OrderByDescending(o => o.CreatedAt)
                                        .Select(o => o.OfferState)
                                        .ToList();

            return new MyOffersViewModel
            {
                 OffersStates = offersStates
            };
        }
    }
}