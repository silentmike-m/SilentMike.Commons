using System;

namespace SilentMike.Core5.Data.Interfaces.Interfaces
{
    public interface ITrackedEntity : IEntity
    {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }
    }
}
