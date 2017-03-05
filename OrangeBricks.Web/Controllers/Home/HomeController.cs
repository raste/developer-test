using System.Web.Mvc;

using OrangeBricks.Web.Attributes;
using OrangeBricks.Web.Controllers.Home.Builders;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Home
{
    public class HomeController : BaseController
    {
        public HomeController(IOrangeBricksContext context) : base(context)
        {
        }

        public ActionResult Index()
        {
            if(User.IsInRole("Buyer"))
            {
                return RedirectToAction("Buyer");
            }

            if(User.IsInRole("Seller"))
            {
                return RedirectToAction("Seller");
            }

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult Seller()
        {
            var builder = new SellerHomeViewModelBuilder(Context);
            var viewModel = builder.Build(UserId);
            return View(viewModel);
        }

        [OrangeBricksAuthorize(Roles = "Buyer")]
        public ActionResult Buyer()
        {
            var builder = new BuyerHomeViewModelBuilder(Context);
            var viewModel = builder.Build(UserId);
            return View(viewModel);
        }
    }
}