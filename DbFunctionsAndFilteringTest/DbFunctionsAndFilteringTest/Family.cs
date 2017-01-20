using System.Collections.Generic;

namespace DbFunctionsAndFilteringTest
{
    public class Family
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Fruit> Fruits { get; set; }
    }
}