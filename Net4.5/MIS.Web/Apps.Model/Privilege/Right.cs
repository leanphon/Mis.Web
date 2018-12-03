using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Model.Privilege
{
    public enum RightType
    {
        Menu,
        URL,
    }
    public class Right
    {

        public long id { get; set; }
        public RightType type { get; set; }

        

    }
}
