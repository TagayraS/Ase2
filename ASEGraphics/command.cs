using ASEGraphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASEGraphics
{

    public class Command : Shape
    {
        /* private Boolean fillEnabled;
         private Color penColor;*/

        Boolean give = false;
        //public Dictionary<string, Variable> variables;

        public Command(Graphics g, Pen pen, int positionx, int positiony) : base(g, pen, positionx, positiony)
        {
        }

        /// <summary>
        /// Draws a line from the current position to the specified coordinates.
        /// </summary>
        /// <param name="command">The Command object.</param>
        /// <param name="xaxis2">The x-coordinate to draw the line to.</param>
        /// <param name="yaxis2">The y-coordinate to draw the line to.</param>

        // Inside the Command class
        public void DrawTo(Command command, int xaxis2, int yaxis2)
        {
            if (xaxis2 < 0 || yaxis2 < 0)
            {
                throw new ArgumentException("Invalid arguments. Coordinates must be non-negative.");
            }

            g = command.g;
            g.DrawLine(command.pen, command.positionx, command.positiony, xaxis2, yaxis2);

            // Update the position to the new endpoint
            command.positionx = xaxis2;
            command.positiony = yaxis2;
        }



        /// <summary>
        /// Sets the fill state based on the provided command.
        /// </summary>
        /// <param name="command">The command ('on' or 'off').</param>
        /// <returns>True if the fill state is 'on', false if 'off'.</returns>
        public Boolean Fill(string command)
        {
            if (command.Equals("on"))   // IF ON
            {
                give = true;
            }
            else if (command.Equals("off")) // ELSE IF OFF
            {
                give = false;
            }
            else
            {
                // Throw ArgumentException for invalid command
                throw new ArgumentException("Invalid fill command", nameof(command));
            }

            return give;
        }


        /// <summary>
        /// Sets the pen color based on the provided color name.
        /// </summary>
        /// <param name="getcolor">The color name.</param>
        /// <param name="pen">The pen object to modify.</param>
        /// <returns>The updated color of the pen.</returns>
        public Color PenColor(string getcolor, Pen pen)
        {
            if (getcolor.Equals("yellow"))
            {
                pen.Color = Color.Yellow;

            }

            else if (getcolor.Equals("red"))
            {
                pen.Color = Color.Red;



            }
            else if (getcolor.Equals("purple"))
            {
                pen.Color = Color.Purple;


            }
            else if (getcolor.Equals("orange"))
            {
                pen.Color = Color.Orange;


            }
            else if (getcolor.Equals("white"))
            {
                pen.Color = Color.White;


            }
            else if (getcolor.Equals("blue"))
            {
                pen.Color = Color.Blue;


            }
            else if (getcolor.Equals("pink"))
            {
                pen.Color = Color.Pink;


            }
            else if (getcolor.Equals("green"))
            {
                pen.Color = Color.Green;


            }
            else
            {
                // Throw an exception for unsupported colors before modifying the pen's color
                throw new ArgumentException($"Invalid color: {getcolor}", nameof(getcolor));
            }


            return pen.Color;
        }


        /// <summary>
        /// Draws a circle on the canvas.
        /// </summary>
        /// <param name="command">The Command object.</param>
        /// <param name="onoroff">True to fill the circle, false to draw the outline.</param>
        /// <param name="radius">The radius of the circle.</param>
        public void DrawCircle(Command command, bool onoroff, object radius)
        {
            if (!int.TryParse(radius.ToString(), out int parsedRadius) || parsedRadius <= 0)
            {
                throw new ArgumentException("Invalid radius value", nameof(radius));
            }

            if (onoroff.Equals(false))
            {
                g.DrawEllipse(command.pen, command.positionx - parsedRadius, command.positiony - parsedRadius, 2 * parsedRadius, 2 * parsedRadius);
            }
            else if (onoroff.Equals(true))
            {
                g.FillEllipse(new SolidBrush(command.pen.Color), command.positionx - parsedRadius, command.positiony - parsedRadius, 2 * parsedRadius, 2 * parsedRadius);
            }
        }

        public void DrawRectangle(Command command, Boolean onandoff, int height, int width)
        {
            if (!int.TryParse(height.ToString(), out int parsedHeight) || parsedHeight <= 0)
            {
                throw new ArgumentException("Invalid height value", nameof(height));
            }

            if (!int.TryParse(width.ToString(), out int parsedWidth) || parsedWidth <= 0)
            {
                throw new ArgumentException("Invalid width value", nameof(width));
            }

            if (onandoff.Equals(true))
            {
                g.FillRectangle(new SolidBrush(command.pen.Color), command.positionx - (parsedWidth / 2), command.positiony - (parsedHeight / 2), parsedWidth, parsedHeight);
            }
            else if (onandoff.Equals(false))
            {
                g.DrawRectangle(command.pen, command.positionx - (parsedWidth / 2), command.positiony - (parsedHeight / 2), parsedWidth, parsedHeight);
            }
        }


        public void DrawTriangle(Command command, bool onoroff, object sidelength)
        {
            if (!int.TryParse(sidelength.ToString(), out int parsedsidelength) || parsedsidelength <= 0)
            {
                throw new ArgumentException("Invalid triangle value", nameof(sidelength));
            }

            if (onoroff.Equals(true))
            {
                float height = (float)(parsedsidelength + Math.Sqrt(3) / 2);
                double halfSide = parsedsidelength / 2.0;

                Point vertex1 = new Point((int)(command.positionx - halfSide), (int)(command.positiony - halfSide / Math.Sqrt(3)));
                Point vertex2 = new Point((int)(command.positionx + halfSide), (int)(command.positiony - halfSide / Math.Sqrt(3)));
                Point vertex3 = new Point(command.positionx, (int)(command.positiony + 2 * halfSide / Math.Sqrt(3)));

                Point[] trianglePoints = new Point[]
                {
            vertex1, vertex2, vertex3
                };

                g.FillPolygon(new SolidBrush(command.pen.Color), trianglePoints);
            }
            else if (onoroff.Equals(false))
            {
                float height = (float)(parsedsidelength + Math.Sqrt(3) / 2);
                double halfSide = parsedsidelength / 2.0;

                Point vertex1 = new Point((int)(command.positionx - halfSide), (int)(command.positiony - halfSide / Math.Sqrt(3)));
                Point vertex2 = new Point((int)(command.positionx + halfSide), (int)(command.positiony - halfSide / Math.Sqrt(3)));
                Point vertex3 = new Point(command.positionx, (int)(command.positiony + 2 * halfSide / Math.Sqrt(3)));

                Point[] trianglePoints = new Point[]
                {
                    vertex1, vertex2, vertex3
                };

                g.DrawPolygon(command.pen, trianglePoints);
            }
        }




    }



}