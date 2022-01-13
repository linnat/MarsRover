using MarsRover.Controllers;
using MarsRover.Models;
using MarsRover.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MarsRover.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod_InitialSearchArea()
        {
            RoverService service = new RoverService();

            int exceptedX = 10;
            int exceptedY = 10;
            int testX = 0;
            int testY = 0;

            var test1 = service.InitialSearchArea(10, 10, out testX, out testY);
            Assert.IsTrue(string.IsNullOrEmpty(test1));
            Assert.AreEqual(exceptedX, testX);
            Assert.AreEqual(exceptedY, testY);

            exceptedX = 0;
            exceptedY = 0;
            testX = 0;
            testY = 0;

            var test2 = service.InitialSearchArea(0, 0, out testX, out testY);
            Assert.IsTrue(test2.Length > 0);
            Assert.AreEqual(exceptedX, testX);
            Assert.AreEqual(exceptedY, testY);

            exceptedX = 0;
            exceptedY = 0;
            testX = 0;
            testY = 0;

            var test3 = service.InitialSearchArea(-10, -10, out testX, out testY);
            Assert.IsTrue(test3.Length > 0);
            Assert.AreEqual(exceptedX, testX);
            Assert.AreEqual(exceptedY, testY);
        }

        [TestMethod]
        public void TestMethod_AddTempRover()
        {
            RoverService service = new RoverService();
            List<RoverViewModel> model = new List<RoverViewModel>();

            int exceptedCount = 1;

            model = service.AddTempRover(model);
            Assert.AreEqual(model.Count, exceptedCount);

            exceptedCount = 2;

            model = service.AddTempRover(model);
            Assert.AreEqual(model.Count, exceptedCount);
        }

        [TestMethod]
        public void TestMethod_GetRoverResult()
        {
            RoverService service = new RoverService();
            List<RoverViewModel> model = new List<RoverViewModel>();

            int limitX = 10;
            int limitY = 10;

            string result = string.Empty;

            result = service.GetRoverResult(model, limitX, limitY);
            Assert.AreEqual(result.Length > 0, true);

            model.Add(new RoverViewModel { Index = 0, X = 5, Y = 5, Orientation = "N", Move = "MMMMM" });
            result = service.GetRoverResult(model, limitX, limitY);
            Assert.AreEqual(result, "Rover1: X=5, Y=10, Orientation=N\r\n");

            model.Add(new RoverViewModel { Index = 1, X = 1, Y = 1, Orientation = "s", Move = "MMMMM" });
            result = service.GetRoverResult(model, limitX, limitY);
            Assert.AreEqual(result, "Rover1: X=5, Y=10, Orientation=N\r\nRover2: X=1, Y=-1, Move no complete, Rover is out of range.\r\n");
        }
    }
}
