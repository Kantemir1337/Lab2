using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Risk
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Object { get; set; }
        public bool ConfBreach { get; set; }
        public bool IntegrBreach { get; set; }
        public bool AvailabBreach { get; set; }
        public string In { get; set; }
        public string Change { get; set; }
    }
}
