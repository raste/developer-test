using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using OrangeBricks.Web.Controllers.Appointments.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Appointments.Builders
{
    public class AppointmentsOnPropertyViewModelBuilder
    {
        private readonly IOrangeBricksContext _context;

        public AppointmentsOnPropertyViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates model which has information for the property and the appointment 
        /// requests made for it. The appointments are sorted by descending based on 
        /// their creation date.
        /// </summary>
        public AppointmentsOnPropertyViewModel Build(int propertyId, string sellerUserId)
        {
            var property = _context.Properties
                                   .Where(p => p.Id == propertyId
                                               && p.SellerUserId == sellerUserId)
                                   .Include(x => x.Appointments)
                                   .SingleOrDefault();

            var viewModel = new AppointmentsOnPropertyViewModel()
            {
                PropertyId = property.Id,
                PropertyType = property.PropertyType,
                StreetName = property.StreetName,
                NumberOfBedrooms = property.NumberOfBedrooms,
                Appointments = new List<PropertyAppointment>()
            };

            if (property.Appointments != null
                && property.Appointments.Count > 0)
            {
                viewModel.Appointments = property.Appointments.Select(a => new PropertyAppointment()
                                                                    {
                                                                            AppointmentId = a.Id,
                                                                            Date = a.Date,
                                                                            Status = a.Status
                                                                    })
                                                               .OrderByDescending(a => a.Date)
                                                               .ToList();
            }

            return viewModel;
        }
    }
}