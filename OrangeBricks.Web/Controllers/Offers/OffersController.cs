using System.Web.Mvc;

using OrangeBricks.Web.Attributes;
using OrangeBricks.Web.Controllers.Offers.Builders;
using OrangeBricks.Web.Controllers.Offers.Commands;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Offers
{
    [Authorize]
    public class OffersController : BaseController
    {
        public OffersController(IOrangeBricksContext context) : base(context)
        {
        }

        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult OnProperty(int id)
        {
            var builder = new OffersOnPropertyViewModelBuilder(Context);
            var viewModel = builder.Build(id);

            return View(viewModel);
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult Accept(AcceptOfferCommand command)
        {
            var handler = new AcceptOfferCommandHandler(Context);

            handler.Handle(command);

            return RedirectToAction("OnProperty", new { id = command.PropertyId });
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult Reject(RejectOfferCommand command)
        {
            var handler = new RejectOfferCommandHandler(Context);

            handler.Handle(command);

            return RedirectToAction("OnProperty", new { id = command.PropertyId });
        }

        [OrangeBricksAuthorize(Roles = "Buyer")]
        public ActionResult MyOffers()
        {
            var builder = new MyOffersViewModelBuilder(Context);

            var viewModel = builder.Build(UserId);

            return View(viewModel);
        }
    }
}