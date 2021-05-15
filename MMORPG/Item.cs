using System;

namespace MMORPG {
    public class Item {
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        //public int Score { get; set; }
        public int Level { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
    }
}