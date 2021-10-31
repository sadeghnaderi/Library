using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.LibraryData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private ILibraryData _libraryData;
        
        public LibraryController(ILibraryData libraryData)
        {
            _libraryData = libraryData;
        }

        [HttpGet]
        [Route("api/GetBooks")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBooks()
        {
            return Ok(_libraryData.GetBooks());
        }

        [HttpPost]
        [Route("api/ReserveBook")]
        [Authorize(Roles = "Admin")]
        public IActionResult ReserveBook(string Username,int BookId)
        {
            return Ok(_libraryData.ReserveBook(Username,BookId));
        }

        [HttpGet]
        [Route("api/ShowActiveReservations")]
        [Authorize(Roles = "Admin")]
        public IActionResult UsersReservation()
        {
            return Ok(_libraryData.UsersReservation());
        }

        [HttpPost]
        [Route("api/ReturnBook")]
        [Authorize(Roles = "Admin")]
        public IActionResult ReturnBook(int BookId, string TotalLateFee)
        {
            return Ok(_libraryData.ReturnBook(BookId, TotalLateFee));
        }
        
    }
}