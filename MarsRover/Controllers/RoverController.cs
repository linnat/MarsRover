using MarsRover.Models;
using MarsRover.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarsRover.Controllers
{
    public class RoverController : Controller
    {
        RoverService roverService = null;

        private const string Rovers = "Rovers";
        private const string LimitX = "LimitX";
        private const string LimitY = "LimitY";
        private const string Result = "Result";

        public RoverController()
        {
            roverService = new RoverService();
        }

        public ActionResult Index()
        {
            // Ensure that there are no TempData
            TempData[Rovers] = null;
            TempData[LimitX] = null;
            TempData[LimitY] = null;
            TempData[Result] = null;

            return View();
        }

        [HttpPost]
        public ActionResult Index(int? x, int? y)
        {
            string result = string.Empty;

            int limitX = 0;
            int limitY = 0;

            // Get search area max coordinates: X, Y
            result = roverService.InitialSearchArea(x, y, out limitX, out limitY);

            if (!string.IsNullOrEmpty(result))
            {
                return Content("<script>alert('" + result + "');window.location = '/Rover/Index';</script>");
            }
            else
            {
                TempData[LimitX] = limitX;
                TempData[LimitY] = limitY;
                return RedirectToAction("SetRovers");
            }
        }

        /// <summary>
        /// Input Rovers' data
        /// </summary>
        /// <returns></returns>
        public ActionResult SetRovers()
        {
            TempData.Keep(LimitX);
            TempData.Keep(LimitY);

            // Set a rover empty data first
            var model = new List<RoverViewModel> { new RoverViewModel { Index = 0 } };

            // If exist TempData, then use TempData.
            if (TempData[Rovers] != null)
            {
                model = (List<RoverViewModel>)TempData[Rovers];
            }
            TempData[Rovers] = model;

            return View(model);
        }

        [HttpPost]
        public ActionResult SetRovers(List<RoverViewModel> model)
        {
            TempData.Keep(LimitX);
            TempData.Keep(LimitY);

            // Get all rovers final coordinates and orientation
            var result = roverService.GetRoverResult(model, (int)TempData[LimitX], (int)TempData[LimitY]);
            // Show the result
            TempData[Result] = result;

            return View(model);
        }

        /// <summary>
        /// Set new rover empty data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTempRover(List<RoverViewModel> model)
        {
            TempData.Keep(LimitX);
            TempData.Keep(LimitY);

            model = roverService.AddTempRover(model);
            TempData[Rovers] = model;
            return RedirectToAction("SetRovers");
        }
    }
}