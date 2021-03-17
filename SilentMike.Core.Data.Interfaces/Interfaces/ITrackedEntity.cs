using System;

namespace SilentMike.Core.Data.Interfaces.Interfaces
{
    public interface ITrackedEntity
    {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }
    }
}
