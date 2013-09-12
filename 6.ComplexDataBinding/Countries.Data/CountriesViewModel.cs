using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countries.Data
{
    public class CountriesViewModel
    {
        public CountriesViewModel()
        {
            this.InitSources();
        }

        private void InitSources()
        {
            using (var context = new WorldEntities())
            {
                this.Countries = context.Countries.Include("Towns").ToList();
                this.Towns = context.Towns.Include("Country").ToList();
            }
        }

        public ICollection<Country> Countries { get; set; }

        public ICollection<Town> Towns { get; set; }

        public string GetLanguages(IEnumerable<Data.Language> languages)
        {
            if (languages.Count() == 0)
            {
                return "No languages selected";
            }

            var builder = new StringBuilder();

            foreach (var lang in languages)
            {
                builder.Append(lang.Name + ", ");
            }
            builder.Length -= 2;

            return builder.ToString();
        }
    }
}
