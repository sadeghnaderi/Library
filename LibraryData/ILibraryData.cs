using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.LibraryData
{
    public interface ILibraryData
    {
        List<Book> GetBooks();

        string ReserveBook(string Username, int BookId);

        List<Reservation> UsersReservation();

        string ReturnBook(int BookId, string TotalLateFee);
    }
}
