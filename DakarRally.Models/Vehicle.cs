using DakarRally.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading;
using static DakarRally.Models.Enums;

namespace DakarRally.Models
{
    public class Vehicle
    {
        private IVehicleFactory vehicleFactory;
        private DrivingSimulation drivingSimulation;
        private VehicleClass vClass;
        private VehicleType? vType;
        private DateTime startTime;
        private StringBuilder sb;

        // event for signaling when the race is done
        private AutoResetEvent RaceIsFinished;    
        
        // delegate for updating vehicles informations during the race
        private Action<Vehicle> UpdateVehicle;

        // delegate for signaling that the vehicle is done
        private Action VehicleIsFinished;

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string TeamName { get; set; }
        [Required]
        public string ModelName { get; set; }
        [Required]
        public DateTime ManufacturingDate { get; set; }

        [Required]
        public VehicleClass Class
        {
            get
            {
                return vClass;
            }
            set
            {
                vClass = value;
                drivingSimulation = vehicleFactory.CreateVehicle(vClass);
                drivingSimulation.ReportMalfunction = this.ReportMalfunction;
                drivingSimulation.UpdateCoveredDistance = this.UpdateCoveredDistance;
            }
        }
        public VehicleType? Type
        {
            get
            {
                return vType;
            }
            set
            {
                drivingSimulation.SetType(value);
                vType = value;
            }
        }

        [Required]
        public string MalfunctionStatistics
        {
            get
            {
                return sb.ToString();
            }
            set
            {
                sb = new StringBuilder(value);
            }
        }

        [Required]
        public int CoveredDistance { get; set; }
        [Required]
        public VehicleStatus Status { get; set; }
        public TimeSpan FinishTime { get; set; }
        [ForeignKey("RaceId")]
        public Guid RaceId { get; set; }
        public Race Race { get; set; }

        internal Vehicle(IVehicleFactory vehicleFactory)
            : this()
        {
            this.vehicleFactory = vehicleFactory;
        }
        public Vehicle()
        {
            Status = VehicleStatus.Ready;
            this.vehicleFactory = new VehicleFactory();
            RaceIsFinished = new AutoResetEvent(false);
            sb = new StringBuilder();
        }

        // Starts driving simulation
        public void StartEngines(object semaphore, CountdownEvent ready, CancellationToken cancellationToken, Action<Vehicle> UpdateVehicle, Action VehicleIsFinished)
        {
            this.UpdateVehicle = UpdateVehicle;
            this.VehicleIsFinished = VehicleIsFinished;
            Thread driving = new Thread(() => drivingSimulation.Drive(new DrivingParameters() { LockObj = semaphore, Ready = ready, RaceIsFinished = this.RaceIsFinished, CancellationToken = cancellationToken }));
            driving.Start();
            Status = VehicleStatus.Racing;
            startTime = DateTime.Now;
            UpdateVehicle?.Invoke(this);
        }

        // Updates covered distance, and if it is equal to race length signaling the race that this vehicle has finished it
        private void UpdateCoveredDistance(int vehicleSpeed)
        {
            CoveredDistance += vehicleSpeed;
            if (CoveredDistance >= Constants.RaceLength)
            {
                CoveredDistance = Constants.RaceLength;
                RaceIsFinished.Set();
                Status = VehicleStatus.Finished;
                VehicleIsFinished?.Invoke();
                FinishTime = DateTime.Now - startTime;
            }

            UpdateVehicle?.Invoke(this);
        }

        // Used for keeping track of malfunction statistics during race
        private void ReportMalfunction(MalfunctionType malfunctionType)
        {
            string severity = String.Empty;
            if (malfunctionType == MalfunctionType.Light)
            {
                severity = "Light";
            }
            else
            {
                severity = "Heavy";
                Status = VehicleStatus.Malfunctioned;
                RaceIsFinished.Set();
                VehicleIsFinished?.Invoke();
            }
            string reportMessage = $"{severity} malfunction happend on distance {CoveredDistance} kilometers.\n";
            sb.Append(reportMessage);

            UpdateVehicle?.Invoke(this);
        }
    }
}
