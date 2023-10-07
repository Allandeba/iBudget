using getQuote.Business;
using getQuote.Models;
using Microsoft.AspNetCore.Mvc;

namespace getQuote.Controllers
{
    public class PersonController : BaseController
    {
        private readonly PersonBusiness _business;

        public PersonController(PersonBusiness business)
        {
            _business = business;
        }

        public async Task<IActionResult> Index(IEnumerable<PersonModel>? people)
        {
            return people == null || people.Count() == 0 ? await Search(null) : View(people);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonModel person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }

            await _business.AddAsync(person);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            PersonModel person = await _business.GetByIdAsync(id);
            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PersonModel person)
        {
            if (person == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return View(person);
            }

            await _business.UpdateAsync(person);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _business.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string? search)
        {
            TempData[Constants.SearchBoxData] = search ?? "";
            IEnumerable<PersonModel>? people = await _business.GetAllLikeAsync(search);
            return View(nameof(Index), people);
        }
    }
}
