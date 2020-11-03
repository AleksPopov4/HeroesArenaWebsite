namespace HeroesArenaWebsite.Web.Areas.Administration.Controllers
{
    using HeroesArenaWebsite.Common;
    using HeroesArenaWebsite.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
