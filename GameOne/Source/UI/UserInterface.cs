namespace GameOne.Source.UI
{
    using Microsoft.Xna.Framework;

    using Renderer;
    using Enumerations;

    public class UserInterface
    {
        public static void DrawSideBar(double hpc, int potions, int depth)
        {
            Output.FillRect(600, 0, 200, 480, Color.White);
            // Healthbar
            double hpx = 610;
            double hpy = 10;
            double hpw = 180;
            double hph = 10;
            hpc *= hpw;
            Output.StrokeRect(hpx, hpy, hpw, hph, Color.Red);
            Output.FillRect(hpx, hpy, hpc, hph, Color.Red);
            // Flasks
            for (int i = 0; i < potions; i++)
            {
                double left = 610 + i * 20;
                double top = 30;
                double width = 16;
                double height = 16;
                Output.FillRect(left, top, width, height, Color.Purple);
                Output.FillRect(left + 4, top - 4, width - 8, 4, Color.Gray);
                Output.StrokeRect(left, top, width, height, Color.Gray, 1);
            }
            Output.DrawText(string.Format($"Depth: {depth}"), 610, 270, Color.Black);
        }

        public static void DrawConsole(string debugInfo, string console)
        {
            // Output debug info
            Output.DrawText(debugInfo, 610, 50, Color.Black);
            Output.DrawText(string.Format($"~/> {console}_"), 10, 450, Color.Black);
        }
    }
}
