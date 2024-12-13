using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Interfaces;
using DataAccess;

namespace ForTesting
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Initializing...");

            // Připojovací řetězec
            string connectionString = "Data Source=C:\\Users\\kalus\\source\\repos\\VIS_KAL0326_PROJEKT\\DataAccess\\kal0326_database.db";

            // Inicializace služeb a repository
            IDatabaseAccess databaseAccess = new DatabaseAccess(connectionString);
            IReservationRepository reservationRepository = new ReservationRepository(databaseAccess);
            IClubRepository clubRepository = new ClubRepository(databaseAccess);
            IUserRepository userRepository = new UserRepository(databaseAccess);

            // Přidání ukázkových dat
            //await AddSampleData(clubRepository, userRepository, reservationRepository);

            // Výpis rezervací
            Console.WriteLine("Listing all reservations:");
            var reservations = await reservationRepository.GetAllReservationsAsync();

            foreach (var reservation in reservations)
            {
                Console.WriteLine($"Reservation ID: {reservation.ReservationId}, " +
                                  $"Date: {reservation.ReservationDate}, " +
                                  $"Number of People: {reservation.NumberOfPeople}, " +
                                  $"Is Confirmed: {reservation.IsConfirmed}, " +
                                  $"State: {reservation.State}, " +
                                  $"Price: {reservation.Price}, " +
                                  $"Club: {reservation.ClubId}, " +
                                  $"User: {reservation.UserId}");
            }

            // Výpis rezervací podle uživatele
            Console.WriteLine("\nReservations for user with ID 1:");
            var userReservations = await reservationRepository.GetReservationsByUserIdAsync(1);
            foreach (var reservation in userReservations)
            {
                Console.WriteLine($"Reservation ID: {reservation.ReservationId}, " +
                                  $"Date: {reservation.ReservationDate}, " +
                                  $"Number of People: {reservation.NumberOfPeople}, " +
                                  $"Is Confirmed: {reservation.IsConfirmed}, " +
                                  $"State: {reservation.State}, " +
                                  $"Price: {reservation.Price}, " +
                                  $"Club: {reservation.ClubId}");
            }
        }

        private static async Task AddSampleData(
            IClubRepository clubRepository,
            IUserRepository userRepository,
            IReservationRepository reservationRepository)
        {
            // Přidání klubu
            var club = new Club
            {
                Name = "Night Club",
                Address = "123 Party Street",
                Description = "A vibrant dance club",
                Type = "Dance",
                Capacity = 200,
                Price = 100,
                Image = "club.jpg",
                Services = "Drinks, DJ, Lounge"
            };
            await clubRepository.AddClubAsync(club);

            // Přidání uživatele
            var user = new User
            {
                Email = "user@example.com",
                Telephone = "123-456-789",
                Login = "user1",
                Password = "password123",
                RegistrationDate = DateTime.Now
            };
            await userRepository.AddUserAsync(user);

            // Přidání rezervace
            var reservation = new Reservation
            {
                ReservationDate = DateTime.Now.AddDays(1),
                NumberOfPeople = 4, // Nová vlastnost
                IsConfirmed = true, // Nová vlastnost
                State = "Confirmed",
                Price = 200, // Nová vlastnost
                ClubId = 1,
                UserId = 1
            };
            await reservationRepository.AddReservationAsync(reservation);

            Console.WriteLine("Sample data added.");
        }
    }
}
