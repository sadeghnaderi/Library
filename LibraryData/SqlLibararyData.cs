using Library.IdentityAuth;
using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.LibraryData
{
    public class SqlLibararyData : ILibraryData
    {
        private readonly UserManager<ApplicationUser> userManager;
        private LibraryContext _libraryContext;
        public SqlLibararyData(UserManager<ApplicationUser> userManager, LibraryContext libraryContext)
        {
            this.userManager = userManager;
            _libraryContext = libraryContext;
        }
        public List<Book> GetBooks()
        {
            return _libraryContext.Book.Include(m => m.BookType).ToList();
        }

        public string ReserveBook(string Username,int BookId)
        {
            var DbUser = _libraryContext.User.SingleOrDefault(C => C.Username == Username);           
            if(DbUser == null)
                return "User Not Found";

            var Book = _libraryContext.Book.SingleOrDefault(C => C.Id == BookId);
            if (Book == null)
                return "Book Not Found";

            if (Book.AvailableQty == 0)
                return "Book Not Exict";

            string RoleId = _libraryContext.UserRoles.SingleOrDefault(C => C.UserId == DbUser.Id).RoleId;
            string Role = _libraryContext.Roles.SingleOrDefault(C => C.Id == RoleId).Name;

            if (Role == "VIPUser" && (_libraryContext.Reservation.Where(m => m.UserId == DbUser.Id && m.ReserveStateId == 1).Count() >= 8 || _libraryContext.Reservation.Where(m => m.UserId == DbUser.Id && m.Book.BookTypeId == Book.BookTypeId).Count() >= 1))
                return "VIP User have 8 Book or have 2 type like the type of this book now";
            else if (Role == "User" && (_libraryContext.Reservation.Where(m => m.UserId == DbUser.Id && m.ReserveStateId == 1).Count() >= 4 || _libraryContext.Reservation.Where(m => m.UserId == DbUser.Id && m.Book.BookTypeId == Book.BookTypeId).Count() >= 2))
                return "User Have 4 Book or have 1 type like the type of this book now";

            DateTime today = DateTime.Today.Date;
            DateTime now = today.Date;

            var Reservation = new Reservation
            {
                BookId = BookId,
                UserId = DbUser.Id,
                ReservedDate = now.ToString(),
                ReserveStateId = 1
            };

            _libraryContext.Reservation.Add(Reservation);
            Book.AvailableQty = --Book.AvailableQty;

            _libraryContext.SaveChanges();

            int ReservationId = _libraryContext.Reservation.Where(c => c.UserId == DbUser.Id).OrderByDescending(p => p.ReservedDate).FirstOrDefault().Id;

            return "Book Reserved By This Confirmation Code :" + Reservation.Id.ToString();
        }

        public List<Reservation> UsersReservation()
        {
            List<Reservation> UsersReservation = _libraryContext.Reservation.Where(m => m.ReserveStateId == 1 ).Include(m => m.User).Include(m => m.Book).ToList();
            return UsersReservation;
        }

        public string ReturnBook(int BookId, string TotalLateFee)
        {
            var ReservationDb = _libraryContext.Reservation.Where(m => m.BookId == BookId && m.ReserveStateId == 1).SingleOrDefault();

            if (ReservationDb == null)
                return "This book is not reserved before";

            DateTime today = DateTime.Today.Date;
            DateTime now = today.Date;

            ReservationDb.ReturnedDate = now.ToString();
            ReservationDb.ReserveStateId = 2;
            ReservationDb.TotalLateFee = TotalLateFee;

            _libraryContext.SaveChanges();

            return "This book is returned successfully";
        }
    }
}
