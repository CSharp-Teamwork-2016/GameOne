namespace GameOne.Source.Entities
{
    using System;
    using GameOne.Source.Enumerations;
    using GameOne.Source.Renderer;

    using Microsoft.Xna.Framework;

    public abstract class Item : Model
    {
        protected Item(string id, Vector2 position, string direction, double radius, Spritesheet sprite, State state, int effect)
            : base(id, position, direction, radius, sprite, state)
        {
            this.Effect = effect;
        }   

        public int Effect { get; set; }
    }
}
