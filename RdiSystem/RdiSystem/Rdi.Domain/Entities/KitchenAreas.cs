using System;
using System.Collections.Generic;
using System.Text;

namespace Rdi.Domain
{
    public class KitchenAreas
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public KitchenAreas(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
