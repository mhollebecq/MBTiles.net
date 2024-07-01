using MBTiles.Parser;
using MBTiles.Web.Models;
using MBTiles.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace MBTiles.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MapService mapService;

        public HomeController(ILogger<HomeController> logger, MapService mapService)
        {
            _logger = logger;
            this.mapService = mapService;
        }

        public IActionResult Index()
        {
            var oui = GetMapInfo();

            return View(oui);
        }

        private MapInfos GetMapInfo()
        {
            return mapService.Get(@"C:\Users\mathi\Desktop\cadastre.mbtiles");
        }

        [Route("/tile/{z}/{x}/{y}.png")]
        public IActionResult Tile(int x, int y, int z)
        {
            var toto = GetMapInfo().GetTile(x, y, z);
            return StatusCode((int)HttpStatusCode.NotFound);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}