# MarsRover
**Only use the first quadrant**  
A squad of robotic rovers are to be landed by NASA on a plateau on Mars.  
A rover's position and location is represented by a combination of x and y co-ordinates and a letter representing one of the four cardinal compass points.  
The plateau is divided up into a grid to simplify navigation.  
An example position might be 0, 0, N, which means the rover is in the bottom left corner and facing North.  
In order to control a rover, NASA sends a simple string of letters. The possible letters are 'L', 'R' and 'M'.  
'L' and 'R' makes the rover spin 90 degrees left or right respectively, without moving from its current spot.  
'M' means move forward one grid point, and maintain the same heading.  
Assume that the square directly North from (x, y) is (x, y+1).  
