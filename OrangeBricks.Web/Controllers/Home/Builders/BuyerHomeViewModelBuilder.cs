using System;
using System.Linq;

using OrangeBricks.Web.Controllers.Home.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Home.Builders
{
    public class BuyerHomeViewModelBuilder
    {
        private readonly IOrangeBricksContext _context;

        public BuyerHomeViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        public BuyerHomeViewModel Build(string userId)
        {
            var offers = _context.Offers.Where(o => o.BuyerUserId == userId)
                                        .ToList();

            var appointments = _context.Appointments.Where(a => a.BuyerUserId == userId
                                                                && a.Date > DateTime.Now)
                                                    .ToList();
            
            return new BuyerHomeViewModel()
            {
                 OffersCount = offers.Count,
                 AcceptedOffersCount = offers.Count(o => o.Status == OfferStatus.Accepted),
                 RejectedOffersCount = offers.Count(o => o.Status == OfferStatus.Rejected),
                 AppointmentsCount = appointments.Count,
                 AcceptedAppointments = appointments.Count(a => a.Status == AppointmentStatus.Accepted),
                 DeclinedAppointments = appointments.Count(a => a.Status == AppointmentStatus.Declined)
            };
        }
    }
}