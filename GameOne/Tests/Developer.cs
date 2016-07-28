namespace GameOne.Tests
{
    using System;
    using Source.Containers;

    public class Developer
    {
        public static void Init()
        {
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
                    GameContainer.level.Player.Respawn();
                    break;
                case "WARP":
                    GameContainer.level.ExitTriggered = true;
                    break;
            }
        }
    }
}
