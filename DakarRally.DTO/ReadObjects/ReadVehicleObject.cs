using System;

namespace DakarRally.DTO.ReadObjects
{
    public class ReadVehicleObject: IComparable<ReadVehicleObject>
    {
        public Guid Id { get; set; }
        public string TeamName { get; set; }

        public string ModelName { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public string Type { get; set; }

        public string Class { get; set; }

        public int CoveredDistance { get; set; }

        public string Status { get; set; }

        public TimeSpan? FinishTime { get; set; }

        public int CompareTo(ReadVehicleObject other)
        {
            if (other == null || (this.FinishTime.HasValue && !other.FinishTime.HasValue))
            {
                return -1;
            }
            else if (!this.FinishTime.HasValue && !other.FinishTime.HasValue)
            {
                return other.CoveredDistance.CompareTo(this.CoveredDistance);
            }
            else if (!this.FinishTime.HasValue && other.FinishTime.HasValue)
            {
                return 1;
            }
            else
            {
                return this.FinishTime.Value.CompareTo(other.FinishTime.Value);
            }
        }
    }
}