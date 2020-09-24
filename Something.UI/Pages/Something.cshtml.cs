using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Something.Application;
using Something.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Something.UI.Pages
{
    public class SomethingModel : PageModel
    {
        private readonly Random _random = new Random();
        private readonly ISomethingCreateInteractor createInteractor;
        private readonly ISomethingReadInteractor readInteractor;
        private readonly AppDbContext ctx;

        [BindProperty]
        public List<Models.Something> SomethingList { get; set; }
        public SomethingModel(ISomethingCreateInteractor createInteractor, ISomethingReadInteractor readInteractor, AppDbContext ctx)
        {
            this.createInteractor = createInteractor;
            this.readInteractor = readInteractor;
            this.ctx = ctx;
            SomethingList = new List<Models.Something>();
        }
        public void OnGet()
        {
            GetAll();
        }
        public void OnPost()
        {
            createInteractor.CreateSomething(RandomString(RandomNumber(10, 20), true));
            GetAll();
        }
        private void GetAll()
        {
            var somethingListResult = readInteractor.GetSomethingList();
            foreach (var something in somethingListResult)
            {
                var st = new Models.Something();
                st.Name = something.Name;
                SomethingList.Add(st);
            }
        }
        private int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        private string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26;

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
