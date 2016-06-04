using System.Collections.Generic;
using System;

namespace GameOne.Tests
{
	public class ListOf
	{
		public static Dictionary<string, Action> tests = new Dictionary<string, Action>();
		public static List<Action> activeTests = new List<Action>();

		public static void Init()
		{
			tests.Add("SAMPLE", Sample);
		}

		static void Sample()
		{
			Source.Loop.DebugInfo += "This is a test";
		}
	}
}
