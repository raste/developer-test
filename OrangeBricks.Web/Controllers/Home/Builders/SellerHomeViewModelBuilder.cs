using System.Linq;

using OrangeBricks.Web.Controllers.Home.ViewModels;
using OrangeBricks.Web.Models;
using System.Data.Entity;

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

            var properties = _context.Properties.Where(p => p.SellerUserId == userId)
                                                .Include(p => p.Offers)
                                                .Include(p => p.Appointments)
                                                .ToList();

            int pendingOffersCount = 0, pendingAppointmentsCount = 0;
            if (properties != null)
            {
                pendingOffersCount = properties.Sum(p => p.Offers.Count(o => o.Status == OfferStatus.Pending));
                pendingAppointmentsCount = properties.Sum(p => p.Appointments.Count(a => a.Status == AppointmentStatus.Pending));
            }

            return new SellerHomeViewModel()
            {
                PendingAppointmentRequests = pendingAppointmentsCount,
                PendingOffers = pendingOffersCount,
                PropertiesCount = propertiesCount
            };
        }
    }
}