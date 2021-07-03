namespace SharedTrip.Controllers
{
    using System.Globalization;
    using System.Linq;


    using Services;
    using Models.Trips;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using SharedTrip.Data;
    using SharedTrip.Data.Models;
    using System;

    public class TripsController : Controller
    {
        private readonly ApplicationDbContext data;

        public TripsController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public HttpResponse Add()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(TripAddInputModel inputModel)
        {
            if (string.IsNullOrWhiteSpace(inputModel.StartPoint))
            {
                return this.Redirect("/Trips/Add");
            }

            if (string.IsNullOrWhiteSpace(inputModel.EndPoint))
            {
                return this.Redirect("/Trips/Add");
            }

            if (string.IsNullOrWhiteSpace(inputModel.Description))
            {
                return this.Redirect("/Trips/Add");
            }

            if (inputModel.Seats < 2 || inputModel.Seats > 6)
            {
                return this.Redirect("/Trips/Add");
            }

            if (inputModel.Description?.Length > 80)
            {
                return this.Redirect("/Trips/Add");
            }

            var trip = new Trip
            {
                StartPoint = inputModel.StartPoint,
                EndPoint = inputModel.EndPoint,
                Description = inputModel.Description,
                DepartureTime = DateTime.Parse(inputModel.DepartureTime),//
                ImagePath = inputModel.ImagePath,
                Seats = inputModel.Seats
            };

            data.Trips.Add(trip);

            data.SaveChanges();

            return this.Redirect("/");
        }

        public HttpResponse All()
        {
            var trips = this.data.Trips
                .Select(t => new TripsAllViewModel
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    Seats = t.Seats - this.data.UsersTrips.Where(x => x.TripId == t.Id).Count(),//
                    DepartureTime = t.DepartureTime
                        .ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture)
                })
                .ToList();

            return this.View(trips);
        }

        public HttpResponse Details(string tripId)
        {

            var trip = this.data.Trips.FirstOrDefault(x => x.Id == tripId);

            if (trip == null)
            {
                return this.Error("Trip not found.");
            }

            var details = new TripDetailsViewModel
            {
                Id = trip.Id,
                StartPoint = trip.StartPoint,
                EndPoint = trip.EndPoint,
                ImagePath = trip.ImagePath,
                Seats = trip.Seats - this.data.UsersTrips.Where(x => x.TripId == trip.Id).Count(),//
                Description = trip.Description,
                DepartureTime = trip.DepartureTime.ToString("dd.MM.yyyy HH:mm")
            };

            return this.View(details);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (this.data.Trips.FirstOrDefault(x => x.Id == tripId) == null)
            {
                return this.Error("Trip not found.");
            }

            var user = this.User;

            if (this.data.UsersTrips.FirstOrDefault(x => x.UserId == user.Id && x.TripId == tripId) != null)
            {
                return this.Redirect($"/Trips/Details?tripId={tripId}");
            }

            var userTrip = new UserTrip
            {
                UserId = user.Id,
                TripId = tripId,
            };

            this.data.UsersTrips.Add(userTrip);
            data.SaveChanges();//

            return this.Redirect("/");
        }
    }
}