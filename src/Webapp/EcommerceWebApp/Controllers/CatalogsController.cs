using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceWebApp.Data;
using EcommerceWebApp.Models;
using EcommerceWebApp.ApiServices.Catalog;

namespace EcommerceWebApp.Controllers
{
    public class CatalogsController : Controller
    {
        private readonly ICatalogService _context;

        public CatalogsController(ICatalogService context)
        {
            _context = context;
        }

        // GET: Catalogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.GetCatalogProducts());
        }

        //// GET: Catalogs/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    //if (id == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //var catalog = await _context.Catalog
        //    //    .FirstOrDefaultAsync(m => m.Id == id);
        //    //if (catalog == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    return View();
        //}
    }
}
