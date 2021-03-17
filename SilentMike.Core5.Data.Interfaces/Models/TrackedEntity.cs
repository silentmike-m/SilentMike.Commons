using SilentMike.Core5.Data.Interfaces.Interfaces;
using System;

namespace SilentMike.Core5.Data.Interfaces.Models
{
    public class TrackedEntity : Entity, ITrackedEntity
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
