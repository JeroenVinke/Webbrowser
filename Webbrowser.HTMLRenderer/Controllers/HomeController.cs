using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Webbrowser.Core;
using Webbrowser.HTMLRenderer.Models;

namespace Webbrowser.HTMLRenderer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var engine = new HtmlEngine();
            engine.SetFilePathResolver(x => AppContext.BaseDirectory + "\\DemoFiles\\" + x);
            var renderTree = engine.RenderRaw(System.IO.File.ReadAllText(AppContext.BaseDirectory + "\\DemoFiles\\index.html"));

            var html = new HTMLGenerator(renderTree).GenerateHTML();

            return new ContentResult
            {
                Content = html,
                ContentType = "text/html",
                StatusCode = 200
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
