using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_HW03.Data;
using Web_HW03.Models;

namespace Web_HW03.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly string uploadsPath;

        public BlogPostsController(ApplicationDbContext context,
                                   IHostingEnvironment hostingEnvironment)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            this.hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            uploadsPath = Path.Combine(hostingEnvironment.WebRootPath, "img");
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogPosts.ToListAsync());
        }

        [HttpGet("friendly/{*slug}")]
        public async Task<IActionResult> Friendly(string slug)
        {
            var match = await _context.BlogPosts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.URLFriendly == slug);
            if (match == null)
                return NotFound();
            return View("Details", match);
        }

        [HttpPost]
        public async Task<IActionResult> ImageUpload(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var file = image;
                //There is an error here
                if (file.Length > 0)
                {
                    var fileName = $"{BlogPost.MakeFriendly(Path.GetFileNameWithoutExtension(file.FileName))}.{Path.GetExtension(file.FileName)}";
                    using (var fileStream = new FileStream(Path.Combine(uploadsPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            return View("Edit");
        }

        // GET: BlogPosts/Details/5
        [HttpGet("posts/{id}")]        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(p=>p.PostTags)
                .ThenInclude(pt=>pt.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Create
        [Authorize(Policy = MyIdentityData.BlogPolicy_Add)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = MyIdentityData.BlogPolicy_Add)]
        public async Task<IActionResult> Create([Bind("Id,Title,Body")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.Posted = DateTime.Now;
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        [Authorize(Policy = MyIdentityData.BlogPolicy_Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            var view = View(blogPost);
            view.ViewData["images"] = Directory.GetFiles(uploadsPath).Select(f=>
            {
                var file = new FileInfo(f);
                return "/img/" + file.Name;
            });

            return view;
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = MyIdentityData.BlogPolicy_Edit)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,Image,TagsString")] BlogPost blogPost, IFormFile image)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null)
                    {
                        var stream = new MemoryStream();
                        await image.CopyToAsync(stream);
                        blogPost.Image = stream.ToArray();
                    }

                    if (!String.IsNullOrWhiteSpace(blogPost.TagsString))
                    {
                        blogPost.PostTags = new List<PostTag>();
                        foreach(var tagText in blogPost.TagsString.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                        {
                            var tag = _context.Tags.Where(t => t.TagName == tagText).FirstOrDefault();
                            if (tag == null)
                                tag = new Tag { TagName = tagText };

                            blogPost.PostTags.Add(new PostTag {PostId=blogPost.Id, Tag=tag });
                        }
                    }
                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        [Authorize(Policy = MyIdentityData.BlogPolicy_Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = MyIdentityData.BlogPolicy_Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }
    }
}
