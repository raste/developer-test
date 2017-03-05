using System.Linq;

using OrangeBricks.Web.Controllers.Home.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Home.Builders
{
    public class SellerHomeViewModelBuilder
    {
        private readonly IOrangeBricksContext _context;

        public SellerHomeViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        public SellerHomeViewModel Build(string userId)
        {
            var propertiesCount = _context.Properties.Count(p => p.SellerUserId == userId);

            var pendingOffersCount = _context.Properties.Where(p => p.SellerUserId == userId)
                                                        .Sum(p => p.Offers.Count(o => o.Status == OfferStatus.Pending));

            var pendingAppointmentsCount = _context.Properties.Where(p => p.SellerUserId == userId)
                                                              .Sum(p => p.Appointments.Count(a => a.Status == AppointmentStatus.Pending));

            return new SellerHomeViewModel()
            {
                PendingAppointmentRequests = pendingAppointmentsCount,
                PendingOffers = pendingOffersCount,
                PropertiesCount = propertiesCount
            };
        }
    }
}