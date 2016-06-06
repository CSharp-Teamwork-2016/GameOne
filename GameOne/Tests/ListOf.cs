using System.Collections.Generic;
using System;

namespace GameOne.Tests
{
	// Този клас зарежда методи, с които да тествате кода си
	// За да добавите нов тест, във метода Init() добавете към речника референция към функцията която искате да се закачи за
	// цикъла и име, с което да я активирате от конзолата. Ползвайте примера, за да се ориенирате. Метода се изпълнява на всеки
	// Update, което е 60 пъти в секунда.
	// В момента единствения видим резултат, който можете да получите е изписване на текст на екрана, чрез добавяне на стринг към
	// Source.Loop.DebugInfo, како е в примера
	public class ListOf
	{
		public static Dictionary<string, Action> tests = new Dictionary<string, Action>();
		public static List<Action> OnDraw = new List<Action>();
		public static List<Action> OnUpdate = new List<Action>();

        public static void Init()
        {
            tests.Add("SAMPLE", Sample);
            tests.Add("SHAPES", DrawShapes);
            // Добавете нови тестове тук. Името на метода се пише без скоби, по този начин го изпращате по рефенция
        }

		static void Sample()
		{
			Source.Loop.DebugInfo += string.Format($"This is a test{Environment.NewLine}");
		}

		public static void ShowFPS()
		{
			Source.Loop.ShowFPS = !Source.Loop.ShowFPS;
		}

        public static void DrawShapes()
        {
            Source.Renderer.Output.FillRect(60, 60, 100, 200);
            Source.Renderer.Output.StrokeRect(80, 90, 200, 100);
        }
    }
}
