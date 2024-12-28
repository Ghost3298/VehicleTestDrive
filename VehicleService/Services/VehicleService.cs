using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.data;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.Services
{
    public class VehicleService : IVehicleService
    {
        private VehicleTestDriveDBContext _dbContext;
        public VehicleService()
        {
            this._dbContext = new VehicleTestDriveDBContext();
        }
        public Task<bool> AddVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new NotImplementedException(nameof(vehicle));
            } else
            {
                var newVehicle = _dbContext.Vehicles.Add(vehicle);
                _dbContext.SaveChanges();
                return Task.FromResult(true);
            }
        }

        public Task<bool> DeleteVehicle(int vehicleId)
        {
           var vehicleToDelete = _dbContext.Vehicles.Find(vehicleId);

            if (vehicleToDelete == null)
            {
                return Task.FromResult(false);
            }

            _dbContext.Vehicles.Remove(vehicleToDelete);
            _dbContext.SaveChanges();
            return Task.FromResult(true);
        }

        public async Task<Vehicle> GetVehicleById(int vehicleId)
        {
            if(vehicleId == null)
            {
                throw new NullReferenceException();
            }

            var vehicle = await _dbContext.Vehicles.FindAsync(vehicleId);
            return vehicle;
        }

        public Task<List<Vehicle>> GetVehicles()
        {
            using (_dbContext)
            {
                return Task.FromResult(_dbContext.Vehicles.ToList());
            }
        }

        public Task<bool> UpdateVehicle(int vehicleId, Vehicle vehicle)
        {
            var vehicleToUpdate = _dbContext.Vehicles.Find(vehicleId);
            
            if (vehicleToUpdate == null)
            {
                return Task.FromResult(false);
            }
            vehicleToUpdate.SpeedUnit = vehicle.SpeedUnit;
            vehicleToUpdate.Price = vehicle.Price;
            vehicleToUpdate.Name = vehicle.Name;
            vehicleToUpdate.ImageUrl = vehicle.ImageUrl;
            vehicleToUpdate.MaxSpeed = vehicle.MaxSpeed;

            _dbContext.Update(vehicleToUpdate);
            _dbContext.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
