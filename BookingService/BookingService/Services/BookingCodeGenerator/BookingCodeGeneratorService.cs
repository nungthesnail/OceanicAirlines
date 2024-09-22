using BookingService.Repository;
using System.Text;

namespace BookingService.Services.BookingCodeGenerator
{
    public class BookingCodeGeneratorService : IBookingCodeGeneratorService
    {
        /// <summary>
        /// Символы, из которых собирается код бронирования
        /// </summary>
        private const string _allSymbols = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Длина кода бронирования
        /// </summary>
        private const int _codeLength = 10;

        private DatabaseBookingFacade _dbBooking = null!;

        private Random _random = null!;

        public BookingCodeGeneratorService()
        {
            InitializeRandomNumberGenerator();
        }

        public void SetDatabaseBookingFacade(DatabaseBookingFacade dbBooking)
        {
            _dbBooking = dbBooking;
        }

        private void InitializeRandomNumberGenerator()
        {
            int seed = (int)DateTime.UtcNow.Ticks;

            _random = new Random(seed);
        }

        public string Generate()
        {
            string result;

            do
            {
                result = TryGenerate();
            }
            while (_dbBooking.Exists(result));

            return result;
        }

        private string TryGenerate()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < _codeLength; ++i)
            {
                sb.Append(GenerateRandomChar());
            }

            return sb.ToString();
        }

        private char GenerateRandomChar()
        {
            var index = _random.Next() % _allSymbols.Length;

            var result = _allSymbols[index];

            return result;
        }
    }
}
