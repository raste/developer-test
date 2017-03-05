using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers
{
    /// <summary>
    /// Contains commonly used functionality from controllers
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly IOrangeBricksContext Context;

        protected string UserId
        {
            get
            {
                if(User.Identity.IsAuthenticated == false)
                {
                    return null;
                }

                return User.Identity.GetUserId();
            }
        }

        public BaseController(IOrangeBricksContext context)
        {
            Context = context;
        }
    }
}