namespace GameOne.Source.Entities.Items
{
    using System;

    public abstract class Item : Entity
    {
        private decimal price;
        private double weight;

        protected Item(string id, decimal price, double weight)
            : base(id)
        {
            this.Price = price;
            this.Weight = weight;
        }

        public decimal Price
        {
            get
            {
                return this.price;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentNullException("The price cannot be negative");
                }

                this.price = value;
            }
        }


        public double Weight
        {
            get
            {
                return this.weight;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentNullException("The weight cannot be negative");
                }

                this.weight = value;
            }
        }
    }
}
