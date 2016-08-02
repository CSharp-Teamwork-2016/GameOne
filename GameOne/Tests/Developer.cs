namespace GameOne.Tests
{
    using System;
    using Source.World;

    public class Developer
    {
        public static Level Level { get; set; }

        public static void Init(Level InLevel)
        {
            Level = InLevel;
            ListOf.Init();
        }

        public static void Exec(string command)
        {
            string[] args = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length < 1)
            {
                return;
            }

            switch (args[0])
            {
                case "FPS":
                    ListOf.ShowFPS();
                    break;
                case "ONDRAW":
                    if (ListOf.tests.ContainsKey(args[1]))
                    {
                        ListOf.OnDraw.Add(ListOf.tests[args[1]]);
                    }

                    break;
                case "ONUPDATE":
                    if (ListOf.tests.ContainsKey(args[1]))
                    {
                        ListOf.OnUpdate.Add(ListOf.tests[args[1]]);
                    }

                    break;
                case "EXEC":
                    if (ListOf.tests.ContainsKey(args[1]))
                    {
                        ListOf.tests[args[1]]();
                    }

                    break;
                case "EXIT":
                    Environment.Exit(0);
                    break;
                case "RESPAWN":
                    Level.Player.Respawn();
                    Level.Entities.Add(Level.Player);
                    break;
                case "WARP":
                    Level.ExitTriggered = true;
                    break;
                case "LVLUP":
                    Level.Player.GainXP(1000);
                    break;
            }
        }
    }
}
