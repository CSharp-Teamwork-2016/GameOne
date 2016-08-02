namespace GameOne.Source.Events
{
    using System;
    using Enumerations;

    public class OnButtonClickEventArgs : EventArgs
    {
        public OnButtonClickEventArgs(GameState gameState)
        {
            this.GameState = gameState;
        }

        public GameState GameState { get; }
    }
}
