using System;
using System.Collections.Generic;

namespace openspace.Common.Entities
{
    public class Session
    {
        public bool AttendanceEnabled { get; set; } = true;

        public string CreatedAt { get; set; } = DateTime.Now.ToShortDateString();

        public bool FreeForAll { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();

        public ICollection<Slot> Slots { get; set; } = new List<Slot>();

        public ICollection<Topic> Topics { get; set; } = new List<Topic>();

        public bool VotingEnabled { get; set; }
    }
}
