using System;

namespace DbFunctionsAndFilteringTest
{
    public class Fruit : IDeleted
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Family { get; set; }
        public DateTime Created { get; set; }
        public bool Deleted { get; set; }
    }
}
