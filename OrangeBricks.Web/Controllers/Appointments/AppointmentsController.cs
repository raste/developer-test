using System.Web.Mvc;

using OrangeBricks.Web.Attributes;
using OrangeBricks.Web.Controllers.Appointments.Builders;
using OrangeBricks.Web.Models;
using OrangeBricks.Web.Controllers.Appointments.Commands;

namespace OrangeBricks.Web.Controllers.Appointments
{
    [Authorize]
    public class AppointmentsController : BaseController
    {
        public AppointmentsController(IOrangeBricksContext context) : base(context)
        {
        }

        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult OnProperty(int id)
        {
            var builder = new AppointmentsOnPropertyViewModelBuilder(Context);

            var viewModel = builder.Build(id, UserId);

            return View(viewModel);
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult Accept(UpdateAppointmentCommand command)
        {
            command.SellerUserId = UserId;
            command.NewStatus = AppointmentStatus.Accepted;

            return UpdateAppointmentStatus(command);
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult Decline(UpdateAppointmentCommand command)
        {
            command.SellerUserId = UserId;
            command.NewStatus = AppointmentStatus.Declined;

            return UpdateAppointmentStatus(command);
        }

        private ActionResult UpdateAppointmentStatus(UpdateAppointmentCommand command)
        {
            var handler = new UpdateAppointmentCommandHandler(Context);

            handler.Handle(command);

            return RedirectToAction("OnProperty", new { id = command.PropertyId });
        }

        [OrangeBricksAuthorize(Roles = "Buyer")]
        public ActionResult MyAppointments()
        {
            var builder = new MyAppointmentsViewModelBuilder(Context);

            var viewModel = builder.Build(UserId);

            return View(viewModel);
        }

    }
}