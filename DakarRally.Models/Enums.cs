namespace DakarRally.Models
{
    public class Enums
    {
        public enum VehicleType
        {
            Sport,
            Terrain,
            Cross
        }

        public enum VehicleClass
        {
            Car,
            Truck,
            Motorbike
        }

        public enum VehicleStatus
        {
            Ready,
            Racing,
            Malfunctioned,
            Finished
        }

        public enum MalfunctionType
        {
            Light,
            Heavy
        }

        public enum RaceStatus
        {
            Pending,
            Running,
            Finished
        }
    }
}
