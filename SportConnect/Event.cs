﻿using System;
using System.Globalization;

namespace SportConnect
{
    [Serializable]
    public class Event
    {
        public Event(int eventId,
                    string name,
                    string sport,
                    string start,
                    string end,
                    int maxPlayers,
                    string skill,
                    string location) 
        {
            Name = name;
            Sport = sport;
            Start = DateTime.Parse(start);
            End = DateTime.Parse(end);
            MaxPlayers = maxPlayers;
            SkillLevel = skill;
            Location = location;
            Id = eventId;

            //How to set Display String
            DisplayString = "Name: " + Name + "\n" +
                            "Sport: " + Sport + "\n" +
                            "Start: " + Start + "\n" +
                            "End: " + End + "\n" +
                            "Maximum Players: " + MaxPlayers + "\n" +
                            "Skill Level: " + SkillLevel + "\n" +
                            "Location: " + Location;
        }
        public Event(
                    string name,
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

        public string DisplayString { get; set; }
    }
}
