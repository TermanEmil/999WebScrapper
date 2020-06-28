using System;

namespace SeleniumSandbox
{
    public class SellableGood
    {
        public SellableGood(string name, string price)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Price = price;
        }

        public string Name { get; }
        public string Price { get; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Price) ? Name : $"{Name}: {Price}";
        }
    }
}
