namespace DemolitionFalcons.Service.Controllers
{
    using Data;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        internal readonly DemolitionFalconsDbContext dbContext;

        public BaseApiController()
        {
            dbContext = new DemolitionFalconsDbContext();
        }
    }
}