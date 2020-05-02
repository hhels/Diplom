using System;
using System.Collections.Generic;
using System.Text;

namespace Diplom.Common.Entities
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Img { get; set; }
        public int Type { get; set; }

    }
}
