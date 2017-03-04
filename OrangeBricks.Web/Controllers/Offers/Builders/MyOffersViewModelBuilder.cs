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

        public MyOffersViewModel Build(string userID)
        {
            var userPropertyOffers = _context.Properties
                                             .Where(p => p.Offers.Any(o => o.BuyerUserId == userID))
                                             .Select(p => new
                                             {
                                                 p.StreetName,
                                                 p.PropertyType,
                                                 Offers = p.Offers.Where(o => o.BuyerUserId == userID)
                                             })
                                             .ToList();

            var offersStates = userPropertyOffers.SelectMany(p => p.Offers.Select(o => new
                                                 {
                                                     p.PropertyType,
                                                     p.StreetName,
                                                     o.Amount,
                                                     o.Status,
                                                     o.CreatedAt
                                                 }))
                                                 .OrderByDescending(o => o.CreatedAt)
                                                 .Select(o => new PropertyOfferState()
                                                 {
                                                     OfferAmount = o.Amount,
                                                     PropertyType = o.PropertyType,
                                                     OfferStatus = o.Status,
                                                     PropertyStreetName = o.StreetName
                                                 })
                                                 .ToList();

            return new MyOffersViewModel
            {
                 OffersStates = offersStates
            };
        }
    }
}