using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class CatchyTeam
    {
        public string teamName = "";
        public List<CatchyPlayer> player { get; set; } = new List<CatchyPlayer>();
    }
}
