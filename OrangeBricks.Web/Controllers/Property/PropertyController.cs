using System.Linq;
using System.Web.Mvc;

using OrangeBricks.Web.Attributes;
using OrangeBricks.Web.Controllers.Property.Builders;
using OrangeBricks.Web.Controllers.Property.Commands;
using OrangeBricks.Web.Controllers.Property.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Property
{
    public class PropertyController : BaseController
    {
        public PropertyController(IOrangeBricksContext context) : base(context)
        {
        }

        [Authorize]
        public ActionResult Index(PropertiesQuery query)
        {
            var builder = new PropertiesViewModelBuilder(Context);

            var isSeller = User.IsInRole("Seller");

            var viewModel = builder.Build(query, isSeller);

            return View(viewModel);
        }

        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult Create()
        {
            var viewModel = new CreatePropertyViewModel();

            viewModel.PossiblePropertyTypes = new string[] { "House", "Flat", "Bungalow" }
                .Select(x => new SelectListItem { Value = x, Text = x })
                .AsEnumerable();

            return View(viewModel);
        }

        [OrangeBricksAuthorize(Roles = "Seller")]
        [HttpPost]
        public ActionResult Create(CreatePropertyCommand command)
        {
            var handler = new CreatePropertyCommandHandler(Context);

            command.SellerUserId = UserId;

            handler.Handle(command);

            return RedirectToAction("MyProperties");
        }

        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult MyProperties()
        {
            var builder = new MyPropertiesViewModelBuilder(Context);
            var viewModel = builder.Build(UserId);

            return View(viewModel);
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult ListForSale(ListPropertyCommand command)
        {
            var handler = new ListPropertyCommandHandler(Context);

            handler.Handle(command);

            return RedirectToAction("MyProperties");
        }

        [OrangeBricksAuthorize(Roles = "Buyer")]
        public ActionResult MakeOffer(int id)
        {
            var builder = new MakeOfferViewModelBuilder(Context);
            var viewModel = builder.Build(id);
            return View(viewModel);
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Buyer")]
        public ActionResult MakeOffer(MakeOfferCommand command)
        {
            command.BuyerUserId = UserId;

            var handler = new MakeOfferCommandHandler(Context);

            handler.Handle(command);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Buyer")]
        public ActionResult BookAppointment(BookAppointmentCommand command)
        {
            command.BuyerUserId = UserId;

            var handler = new BookAppointmentCommandHandler(Context);

            handler.Handle(command);

            return RedirectToAction("Index");
        }
    }
}