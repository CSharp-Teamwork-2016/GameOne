﻿namespace GameOne.Source.World.Physics
{
    using System.Windows;
    using Enumerations;
    using Interfaces;

    public class MovableBody : RigidBody, IMovable
    {
        private Vector velocity;

        public Vector Velocity
        {
            get
            {
                return this.velocity;
            }
            set
            {
                this.velocity = value;
            }
        }

        public MovementType MovementType
        {
            get
            {
                return MovementType.Normal;
            }
        }

        public void Accelerate(Vector acceleration)
        {
            this.Velocity += acceleration;
        }

        public void TurnTo(double direction)
        {
            this.Direction = direction;
        }

        public void Stop()
        {
            this.Velocity = new Vector(0, 0);
        }
    }
}
