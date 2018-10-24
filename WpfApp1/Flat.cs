using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Flat
    {
        public int id { get; set; }
        public string title { get; set; }
        public int number { get; set; }
        public Nullable<int> price { get; set; }
        public Nullable<int> rooms { get; set; }
        public Nullable<int> size { get; set; }
        public Nullable<int> floor { get; set; }
    }
}
