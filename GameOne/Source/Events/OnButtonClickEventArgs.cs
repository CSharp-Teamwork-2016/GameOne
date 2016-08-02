namespace GameOne.Source.Events
{
    using System;
    using Enumerations;

    public class OnButtonClickEventArgs : EventArgs
    {
        public OnButtonClickEventArgs(GameState gameState)
            : this(gameState, string.Empty)
        {
        }

        public OnButtonClickEventArgs(GameState gameState, string name)
        {
            this.GameState = gameState;
            this.Name = name;
        }

        public GameState GameState { get; }

        public string Name { get; }
    }
}
