using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_HW03.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_HW03.Controllers
{
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext context;

        public TagsController(ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TagList(int id)
        {
            try
            {
                if (!context.Tags.Any(t => t.Id == id))
                    return NotFound();

                var tag = context.Tags
                    .Include(t => t.PostTags)
                    .ThenInclude(pt=>pt.Post)
                    .Single(t => t.Id == id);
                
                return View("Index", tag.PostTags);
            }
            catch(Exception ex)
            {
                throw;
            }
            //return View();
        }
    }
}
