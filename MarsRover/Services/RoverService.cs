using MarsRover.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MarsRover.Service
{
    public class RoverService
    {
        public RoverService()
        {

        }

        /// <summary>
        /// Set search area max coordinates: X, Y
        /// </summary>
        /// <param name="x">User input</param>
        /// <param name="y">User input</param>
        /// <param name="limitX">Output X</param>
        /// <param name="limitY">Output Y</param>
        /// <returns>Error Message</returns>
        public string InitialSearchArea(int? x, int? y, out int limitX, out int limitY)
        {
            string result = string.Empty;

            limitX = 0;
            limitY = 0;

            if (x == null || y == null)
            {
                result = "X or Y Coordinate must be an integer.";
            }
            else if (x == 0 && y == 0)
            {
                result = "X or Y Coordinate at least greater than 0.";
            }
            else if (x < 0)
            {
                result = "X Coordinate should be greater than or equal to 0.";
            }
            else if (y < 0)
            {
                result = "Y Coordinate should be greater than or equal to 0.";
            }
            else
            {
                limitX = x.Value;
                limitY = y.Value;
            }

            return result;
        }

        /// <summary>
        /// Add new rover empty data
        /// </summary>
        /// <param name="model">Current rover list</param>
        /// <returns></returns>
        public List<RoverViewModel> AddTempRover(List<RoverViewModel> model)
        {
            // If model is empty
            if (model.Count == 0)
            {
                model.Add(new RoverViewModel { Index = 0 });
                return model;
            }

            // Get new RoverViewModel.Index
            int newIndex = model.Max(x => x.Index) + 1;
            RoverViewModel tempRoverViewModel = new RoverViewModel { Index = newIndex };
            model.Add(tempRoverViewModel);

            return model;
        }

        /// <summary>
        /// Handle all rovers rotation and movement
        /// </summary>
        /// <param name="model">Current rover list</param>
        /// <param name="limitX">Search limit for x coordinates</param>
        /// <param name="limitY">Search limit for y coordinates</param>
        /// <returns></returns>
        public string GetRoverResult(List<RoverViewModel> model, int limitX, int limitY)
        {
            StringBuilder result = new StringBuilder();

            if (model.Count == 0)
            {
                result.AppendLine("At least need one Rover's data.");
            }
            if (result.Length > 0)
            {
                return result.ToString();
            }

            #region Double Check the inputs are legal or not
            var orientationWord = new Regex(@"^[NESW]$");
            var moveWord = new Regex(@"^[LRM]*$");

            if (model.Any(x => !orientationWord.IsMatch(x.Orientation.ToUpper())))
            {
                result.AppendLine("Rover's orientation must be the following words: N(n), E(e), S(s), W(w)");
            }
            if (model.Any(x => !moveWord.IsMatch(x.Move.ToUpper())))
            {
                result.AppendLine("Rover's move must be composed of the following words: L(l),R(r), M(m)");
            }
            if (result.Length > 0)
            {
                return result.ToString();
            }
            #endregion Double Check the inputs are legal or not

            int i = 1;
            foreach (var rover in model)
            {
                // I hope that the screen maintains the data entered by the user, so RoverPositionModel is created for operations.
                var roverNewPosition = new RoverPositionModel { X = rover.X, Y = rover.Y, Orientation = rover.Orientation.ToUpper() };
                string error = string.Empty;
                foreach (var move in rover.Move.ToUpper())
                {
                    switch (move)
                    {
                        // Rotate
                        case 'L':
                        case 'R':
                            roverNewPosition = RoverRotate(move, roverNewPosition);
                            break;
                        // Move
                        case 'M':
                            error = RoverMove(roverNewPosition, limitX, limitY);
                            break;
                    }
                    // Out of range
                    if (!string.IsNullOrEmpty(error))
                    {
                        break;
                    }
                }
                if (string.IsNullOrEmpty(error))
                {
                    // Show the final position.
                    result.AppendLine("Rover" + i + ": X=" + roverNewPosition.X + ", Y=" + roverNewPosition.Y + ", Orientation=" + roverNewPosition.Orientation);
                }
                else
                {
                    // Show the illegal position with error message.
                    result.AppendLine("Rover" + i + ": X=" + roverNewPosition.X + ", Y=" + roverNewPosition.Y + ", Move no complete, " + error);
                }
                i++;
            }

            return result.ToString();
        }

        /// <summary>
        /// Rover Rotate. Assumed that N=0, E=1, S=2, W=3
        /// </summary>
        /// <param name="turn">L or R</param>
        /// <param name="rover"></param>
        /// <returns></returns>
        private RoverPositionModel RoverRotate(char turn, RoverPositionModel rover)
        {
            int rotate = 0;

            switch (turn)
            {
                case 'L':
                    rotate = 3;
                    break;
                case 'R':
                    rotate = 1;
                    break;
            }

            int dirInt = Direction2Int(rover.Orientation);
            // Assumed that N=0, E=1, S=2, W=3. If turn L, the result orientation will be (x+3)%4, on the other hand, turn R will be (x+1)%4
            dirInt = (dirInt + rotate) % 4;
            rover.Orientation = Int2Direction(dirInt);

            return rover;
        }

        /// <summary>
        /// Rover moves. Area max coordinates must be considered.
        /// </summary>
        /// <param name="rover"></param>
        /// <param name="limitX">max coordinate x</param>
        /// <param name="limitY">max coordinate y</param>
        /// <returns></returns>
        private string RoverMove(RoverPositionModel rover, int limitX, int limitY)
        {
            string result = string.Empty;

            switch (rover.Orientation)
            {
                case "N":
                    rover.Y++;
                    break;
                case "E":
                    rover.X++;
                    break;
                case "S":
                    rover.Y--;
                    break;
                case "W":
                    rover.X--;
                    break;
            }

            // Check the result position is illegal or not.
            if (rover.X < 0 || rover.Y < 0 || rover.X > limitX || rover.Y > limitY)
            {
                result = "Rover is out of range.";
            }

            return result;
        }

        private int Direction2Int(string dircetion)
        {
            int result = 0;

            switch (dircetion)
            {
                case "N":
                    result = 0;
                    break;
                case "E":
                    result = 1;
                    break;
                case "S":
                    result = 2;
                    break;
                case "W":
                    result = 3;
                    break;
            }

            return result;
        }

        private string Int2Direction(int dirInt)
        {
            string result = string.Empty;

            switch (dirInt)
            {
                case 0:
                    result = "N";
                    break;
                case 1:
                    result = "E";
                    break;
                case 2:
                    result = "S";
                    break;
                case 3:
                    result = "W";
                    break;
            }

            return result;
        }
    }
}