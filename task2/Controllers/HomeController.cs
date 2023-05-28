using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using task2.DAL;
using task2.Models;

namespace task2.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbcontext _appDbcontext;

        public HomeController(AppDbcontext appDbcontext)
        {
            _appDbcontext = appDbcontext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _appDbcontext.Teams.Where(t => !t.IsDelected).ToListAsync());
        }

      
    }
}