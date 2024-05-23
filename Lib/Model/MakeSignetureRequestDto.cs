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
        public int team_id { get; set; }
        [DataMember(Name = "game_id")]
        public int game_id { get; set; }
        [DataMember(Name = "team_name")]
        public string team_name { get; set; }
        [DataMember(Name = "player_mobiles")]
        public List<string> player_mobiles { get; set; } = new List<string>();
        public int score { get; set; }
        [DataMember(Name = "date_time")]
        public long date_time { get; set; }
    }
}
