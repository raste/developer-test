using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using OrangeBricks.Web.Attributes;
using OrangeBricks.Web.Controllers.Appointments.Builders;
using OrangeBricks.Web.Models;
using OrangeBricks.Web.Controllers.Appointments.Commands;

namespace OrangeBricks.Web.Controllers.Appointments
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IOrangeBricksContext _context;

        public AppointmentsController(IOrangeBricksContext context)
        {
            _context = context;
        }

        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult OnProperty(int id)
        {
            var builder = new AppointmentsOnPropertyViewModelBuilder(_context);

            var userId = User.Identity.GetUserId();

            var viewModel = builder.Build(id, userId);

            return View(viewModel);
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult Accept(UpdateAppointmentCommand command)
        {
            command.SellerUserId = User.Identity.GetUserId();
            command.NewStatus = AppointmentStatus.Accepted;

            return UpdateAppointmentStatus(command);
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult Decline(UpdateAppointmentCommand command)
        {
            command.SellerUserId = User.Identity.GetUserId();
            command.NewStatus = AppointmentStatus.Declined;

            return UpdateAppointmentStatus(command);
        }

        private ActionResult UpdateAppointmentStatus(UpdateAppointmentCommand command)
        {
            var handler = new UpdateAppointmentCommandHandler(_context);

            handler.Handle(command);

            return RedirectToAction("OnProperty", new { id = command.PropertyId });
        }

        [OrangeBricksAuthorize(Roles = "Buyer")]
        public ActionResult MyAppointments()
        {
            var builder = new MyAppointmentsViewModelBuilder(_context);

            var userId = User.Identity.GetUserId();

            var viewModel = builder.Build(userId);

            return View(viewModel);
        }

    }
}