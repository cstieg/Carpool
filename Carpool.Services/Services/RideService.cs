using Carpool.Domain.Models;
using Carpool.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Carpool.Services.Services
{
    public class RideService
    {
        private readonly IUnitOfWork _uow;

        public RideService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // RIDES **********************
        public async Task<List<Ride>> FindMatchingRidesAsync(Ride ride, float distanceWithin, TimeSpan timeWithin)
        {
            throw new NotImplementedException();
        }

        public async Task<Ride> AddRideRequestToOfferAsync(Ride rideOffer, Ride rideRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<Ride> AddRideAsync(Ride ride)
        {
            _uow.Rides.Add(ride);
            await _uow.SaveChangesAsync();
            return ride;
        }

        public async Task<Ride> GetRideByIdAsync(int id)
        {
            return await _uow.Rides.GetSingleAsync(id);
        }

        public async Task<Ride> EditRideAsync(Ride ride)
        {
            _uow.Rides.Edit(ride);
            await _uow.SaveChangesAsync();
            return ride;
        }

        public async Task DeleteRideAsync(Ride ride)
        {
            // TODO: Do we need to delete the Locations too?
            _uow.RideLegs.DeleteAll(ride.RideLegs);
            _uow.Riders.DeleteAll(ride.Riders);

            if (ride.RideCost != null && !(await RideCostIsUsedForOtherRideAsync(ride.RideCost, ride)))
            {
                _uow.RideCosts.Delete(ride.RideCost);
            }

            _uow.Rides.Delete(ride);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteRideAsync(int id)
        {
            var ride = await _uow.Rides.GetSingleAsync(id);
            await DeleteRideAsync(ride);
        }

        // RIDE LEGS ******************
        public async Task<Ride> AddRideLegsAsync(Ride ride, params RideLeg[] rideLegs)
        {
            foreach (RideLeg rideLeg in rideLegs)
            {
                ValidateRideLeg(rideLeg);

                // Todo: Set time and distance for each leg
                // Can reuse calculated distance for return trip if origin and distination are the same

                _uow.RideLegs.Add(rideLeg);
                ride.RideLegs.Add(rideLeg);
            }
            await _uow.SaveChangesAsync();
            return ride;
        }

        public async Task<RideLeg> EditRideLegAsync(RideLeg rideLeg)
        {

            // Todo: update time and distance if location has changed

            ValidateRideLeg(rideLeg);
            _uow.RideLegs.Edit(rideLeg);
            await _uow.SaveChangesAsync();
            return rideLeg;
        }
        
        public async Task DeleteRideLegAsync(RideLeg rideLeg)
        {
            // TODO: Do we need to delete the Locations too?

            _uow.RideLegs.Delete(rideLeg);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteRideLegAsync(int id)
        {
            var rideLeg = await _uow.RideLegs.GetSingleAsync(id);
            await DeleteRideLegAsync(rideLeg);
        }

        private void ValidateRideLeg(RideLeg rideLeg)
        {
            if (rideLeg.StartTime >= new TimeSpan(24, 0, 0))
            {
                throw new ArgumentOutOfRangeException("StartTime must be less than 24:00:00");
            }
        }


        // RIDERS *********************
        public async Task<Rider> AddRiderAsync(Ride ride, User user)
        {
            var rider = await _uow.Riders.FindBy(r => r.RideId == ride.Id && r.UserId == user.Id).FirstOrDefaultAsync();
            if (rider != null)
            {
                return rider;
            }

            rider = new Rider()
            {
                Ride = ride,
                User = user
            };
            _uow.Riders.Add(rider);
            await _uow.SaveChangesAsync();
            return rider;
        }

        public async Task<List<Rider>> GetRidersForRideAsync(Ride ride)
        {
            return await _uow.Riders.FindBy(r => r.RideId == ride.Id).ToListAsync();
        }

        public async Task DeleteRiderAsync(Rider rider)
        {
            _uow.Riders.Delete(rider);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteRiderAsync(int id)
        {
            var rider = await _uow.Riders.GetSingleAsync(id);
            await DeleteRiderAsync(rider);
        }

        // RIDECOSTS ******************        
        public async Task<RideCost> SetDefaultRideCostForUserAsync(User user, RideCost rideCost)
        {
            _uow.RideCosts.Upsert(rideCost);
            user.DefaultRideCost = rideCost;
            _uow.Users.Edit(user);

            await _uow.SaveChangesAsync();
            return rideCost;
        }

        public async Task<RideCost> SetRideCostForRideAsync(Ride ride, RideCost rideCost)
        {
            _uow.RideCosts.Upsert(rideCost);
            ride.RideCost = rideCost;
            _uow.Rides.Edit(ride);

            await _uow.SaveChangesAsync();
            return rideCost;
        }

        public async Task<decimal> GetRideCostAsync(Ride rideOffer, Ride rideRequest)
        {
            var rideCost = rideOffer.RideCost;
            decimal cost = rideCost.BaseCost;

            var newRide = await AddRideRequestToOfferAsync(rideOffer, rideRequest);
            var extraDistance = newRide.Distance - rideOffer.Distance;
            var extraTime = newRide.TravelTime - rideOffer.TravelTime;

            cost += rideCost.CostPerMile * (decimal)newRide.Distance;
            cost += rideCost.PickupCostPerMile * (decimal)extraDistance;
            cost += rideCost.PickupCostPerHour * (decimal)extraTime.TotalHours;

            return cost;
        }

        private async Task<bool> RideCostIsUsedForOtherRideAsync(RideCost rideCost, Ride exceptRide)
        {
            var rideCostIsDefaultForUser = await _uow.Users.FindBy(u => u.DefaultRideCostId == rideCost.Id).AnyAsync();
            var rideCostIsUsedForOtherRide = await _uow.Rides.FindBy(r => r.RideCostId == rideCost.Id && r.Id != exceptRide.Id).AnyAsync();
            return (!rideCostIsDefaultForUser && !rideCostIsUsedForOtherRide);
        }
    }
}
