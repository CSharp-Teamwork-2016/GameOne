namespace GameOne.Tests
{
    using System;
    using System.Collections.Generic;

    // Този клас зарежда методи, с които да тествате кода си
    // За да добавите нов тест, във метода Init() добавете към речника референция към функцията която искате да се закачи за
    // цикъла и име, с което да я активирате от конзолата. При извикване с ONUPDATE, се изпълнява 60 пъти в секунда в блока
    // Loop.Update(). При извикване с ONDRAW се изпълнява 60 пъти в секунда в блока Loop.Render(). При извикване с EXEC се
    // изпълнява само веднъж - ако искате видим резултат, трябва допълнителна функция да се прикачи към Render().
    public class ListOf
    {
        internal static Dictionary<string, Action> tests = new Dictionary<string, Action>();
        internal static List<Action> OnDraw = new List<Action>();
        internal static List<Action> OnUpdate = new List<Action>();

        internal static void Init()
        {
            tests.Add("SAMPLE", Sample);
            tests.Add("SHAPES", DrawShapes);
            tests.Add("LINES", DrawLines);
            tests.Add("OVAL", DrawOval);
            // Добавете нови тестове тук. Името на метода се пише без скоби, по този начин го изпращате по рефенция
        }

        internal static void Sample()
        {
            Source.Loop.DebugInfo += string.Format($"This is a test{Environment.NewLine}");
        }

        internal static void ShowFPS()
        {
            Source.Loop.ShowFPS = !Source.Loop.ShowFPS;
        }

        internal static void DrawShapes()
        {
            Source.Renderer.Output.StrokeRect(1, 1, 11, 11);
            Source.Renderer.Output.FillRect(1, 1, 11, 11);
            Source.Renderer.Output.DrawLine(1, 1, 11, 11, Microsoft.Xna.Framework.Color.Black, 1);
        }

        internal static void DrawLines()
        {
            Source.Renderer.Output.DrawLine(10, 10, 100, 150, 2);
        }

        internal static void DrawOval()
        {
            Source.Renderer.Output.FillRect(10, 10, 201, 101);
            Source.Renderer.Output.FillOval(10, 10, 201, 101, Microsoft.Xna.Framework.Color.Black);
            Source.Renderer.Output.StrokeOval(10, 10, 201, 101, Microsoft.Xna.Framework.Color.Red, 3);
        }
    }
}
