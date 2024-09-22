using FrontendService.Services.Flights;
using FrontendService.ViewModels.Booking;
using FrontendService.ViewModels.Flights;
using InterserviceCommunication.Models.BookingService;


namespace FrontendService.Services.Booking
{
	/// <summary>
	/// Строитель модели вида данных о бронировании
	/// </summary>
	public class BookingViewModelBuilderService : IBookingViewModelBuilderService
	{
		private readonly IGetFlightService _getFlightService;

		private string? _code = null;

		private FlightViewModel? _flight = null!;

		private IEnumerable<PassengerViewModel> _passengers = [];

		private bool _confirmed = false;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="getFlightService">Сервис получения данных о запланированном рейса</param>
		public BookingViewModelBuilderService(IGetFlightService getFlightService)
		{
			_getFlightService = getFlightService;
		}

		public void SetCode(string code)
		{
			_code = code;
		}

		public void SetFlight(FlightViewModel flight)
		{
			_flight = flight;
		}

		public void SetPassengers(IEnumerable<PassengerViewModel> passengers)
		{
			_passengers = passengers;
		}

		public void SetPassengersFromBookingServicesModels(IEnumerable<BookingServicePassengerToBookingModel> serviceModels)
		{
			_passengers = from p in serviceModels
						  select PassengerViewModel.BuildFrom(p);
		}

		public async Task SetFlightFromBookingService(int flightId)
		{
			var flightModel = await _getFlightService.Get(flightId);

			_flight = FlightViewModel.BuildFrom(flightModel);
		}

		public void SetConfirmed(bool value)
		{
			_confirmed = value;
		}

		public BookingViewModel Build()
		{
			ThrowIfBuildingIsntCompleted();

			return new BookingViewModel
			{
				Code = _code!,
				Flight = _flight!,
				Passengers = _passengers
			};
		}

		private void ThrowIfBuildingIsntCompleted()
		{
			var codeSpecified = _code != null;
			var flightSpecified = _flight != null;
			var passengersSpecified = _passengers.Any();

			var buildingCompleted = codeSpecified && flightSpecified && passengersSpecified;

			if (!buildingCompleted)
			{
				throw new InvalidOperationException();
			}
		}

		public async Task<BookingViewModel> BuildFrom(BookingServiceBookingModel bookingModel)
		{
			var code = bookingModel.Code;
			var flightId = bookingModel.FlightId;
			var confirmed = bookingModel.Confirmed;
			var passengersModels = bookingModel.Passengers;

			SetCode(code);
			await SetFlightFromBookingService(flightId);
			SetConfirmed(confirmed);
			SetPassengersFromBookingServicesModels(passengersModels);

			var result = Build();

			return result;
		}
	}
}
