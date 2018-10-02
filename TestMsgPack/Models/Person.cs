using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMsgPack.Models
{
    public class Person
    {
        public int ID { get; set; }
        
        public string Name { get; set; }
        
        public int Age { get; set; }
        
        public DateTime Date { get; set; }

        
        public Vehicle Vehicle { get; set; }
        
        public override string ToString()
        {
            return $"{Name}[ID: {ID}] has {Age} years old. He's working on vehicle '{Vehicle}' since {Date}.";
        }
    }
}
