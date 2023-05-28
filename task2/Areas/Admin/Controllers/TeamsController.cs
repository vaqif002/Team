using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using task2.Areas.Admin.Viewmodel;
using task2.DAL;
using task2.Models;
using task2.Utilities.Constrants;

namespace task2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamsController : Controller
    {
        private readonly AppDbcontext _appdbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamsController(AppDbcontext appdbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _appdbcontext = appdbcontext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _appdbcontext.Teams.Where(t => !t.IsDelected).OrderByDescending(t=>t.id).ToListAsync());

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreatedTeamVM teamVM)
        {
            if (!ModelState.IsValid) { return View(teamVM); }

            if (!teamVM.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", ErrorMessage.FileMustBeTypeImage);
                return View(teamVM);
            }
            if (teamVM.Photo.Length / 1024 > 200)
            {
                ModelState.AddModelError("Photo", ErrorMessage.FileSizeMustlessThan200Kb);
                return View(teamVM);
            }
            string Rootpath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img","team");
            string filename = Guid.NewGuid().ToString() + teamVM.Photo.FileName;
            using (FileStream fileStream = new FileStream(Path.Combine(Rootpath, filename), FileMode.Create))
            {
                await teamVM.Photo.CopyToAsync(fileStream);

            }
            Team team = new Team()
            {
                Name = teamVM.Name,
                Description = teamVM.Description,
                ImagePath = filename
            };
            await _appdbcontext.Teams.AddAsync(team);
            await _appdbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Delete(int id)
        {
            Team? team = await _appdbcontext.Teams.FindAsync(id);
            if (team == null) { return NotFound(); }
            string filename = Path.Combine(_webHostEnvironment.WebRootPath, "assest", "img", team.ImagePath);
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
            _appdbcontext.Teams.Remove(team);
            await _appdbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public  IActionResult Update(int id)
        {
       
            return View(new UpdateTeamVM());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateTeamVM updateTeam)
        {
            if (!ModelState.IsValid) { return View(updateTeam); }
     

            if (!updateTeam.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", ErrorMessage.FileMustBeTypeImage);
                return View(updateTeam);
            }
            if (updateTeam.Photo.Length / 1024 > 200)
            {
                ModelState.AddModelError("Photo", ErrorMessage.FileSizeMustlessThan200Kb);
                return View(updateTeam);
            }
            Team? teams = (await _appdbcontext.Teams.FindAsync(updateTeam.Id));
            if ( teams== null)
            {
                return NotFound();
            }

            string rootpath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team");
          
             string Oldfilepath=Path.Combine(rootpath, teams.ImagePath);
            if (System.IO.File.Exists(Oldfilepath))
            {
                System.IO.File.Delete(Oldfilepath);
            }
            string newFilename = Guid.NewGuid().ToString() + updateTeam.Photo.FileName;
            string resultpath=Path.Combine(rootpath,newFilename);
            using (FileStream filestream = new(resultpath, FileMode.Create))
            {
                await updateTeam.Photo.CopyToAsync(filestream);
            }
            teams.Name = updateTeam.Name;
            teams.Description = updateTeam.Description;
            teams.ImagePath = newFilename;

            await _appdbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

           






        }
    }
}