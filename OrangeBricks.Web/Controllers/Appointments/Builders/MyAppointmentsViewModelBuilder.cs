using System.Linq;

using OrangeBricks.Web.Controllers.Appointments.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Appointments.Builders
{
    public class MyAppointmentsViewModelBuilder
    {
        private readonly IOrangeBricksContext _context;

        public MyAppointmentsViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        public MyAppointmentsViewModel Build(string buyerUserId)
        {
            var appointmentStates = _context.Properties
                                           .Where(p => p.Appointments.Any(a => a.BuyerUserId == buyerUserId))
                                           .SelectMany(p => p.Appointments.Where(a => a.BuyerUserId == buyerUserId)
                                                                          .Select(a => new
                                                                          {
                                                                              p.PropertyType,
                                                                              p.StreetName,
                                                                              a.Date,
                                                                              a.Status
                                                                          }))
                                           .OrderByDescending(a => a.Date)
                                           .ToList()
                                           .Select(a => new PropertyAppointmentState()
                                           {
                                               AppointmentDate = a.Date,
                                               AppointmentStatus = a.Status,
                                               PropertyType = a.PropertyType,
                                               PropertyStreetName = a.StreetName
                                           })
                                           .ToList();


            var viewModel = new MyAppointmentsViewModel()
            {
                 Appointments = appointmentStates
            };

            return viewModel;
        }
    }
}