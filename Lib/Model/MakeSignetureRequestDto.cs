using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class MakeSignetureRequestDto
    {
        [DataMember(Name = "team_id")]
        public int TeamId { get; set; }
        [DataMember(Name = "game_id")]
        public int GameId { get; set; }
        [DataMember(Name = "team_name")]
        public string TeamName{ get; set; }
        [DataMember(Name = "player_mobiles")]
        public List<string> PlayersMobile { get; set; } = new List<string>();
        public int Score { get; set; }
        [DataMember(Name = "date_time")]
        public long TimeInUnix { get; set; }
    }
}
