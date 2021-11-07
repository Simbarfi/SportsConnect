using System;
using System.Globalization;

namespace SportConnect
{
    public class Event
    {
        public Event() { }
        public Event(string name,
                    string sport,
                    string start,
                    string end,
                    int maxPlayers,
                    string skill, 
                    string location)
        {
            Name = name;
            Sport = sport;
            string format = "d";
            DateTime result;
            DateTime.TryParseExact(start,
                                    format,
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out result);
            Start = result;
            DateTime.TryParseExact(end,
                                    format,
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out result);
            End = result;
            MaxPlayers = MaxPlayers;
            SkillLevel = skill;
            Location = location;
        }
        public int Id {get; set;}
        public string Name { get; set; }
        public string Sport { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int MaxPlayers { get; set; }
        public string SkillLevel { get; set; }
        public string Location { get; set; }
    }
}
