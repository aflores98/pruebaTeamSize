using System.Diagnostics;
using System.Runtime.Serialization;

namespace pruebaTeamSize.Models
{
    
    public class Input
    {
        public Input()
        {
            this.teamSize = new List<int>();
            this.k = 0;

        }

        
        public List<int> teamSize { get; set; }

        
        public int k { get; set; }
    }
}
