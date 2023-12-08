using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.repository
{
    public class CountryReposotry : ICountryReposotry
    {
        private DataContext _context;
        public CountryReposotry(DataContext context)
        {
            _context = context;

        }
        public bool CountryExsits(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(s => s.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int OwnerId)
        {
            return _context.Owners.Where(s => s.Id == OwnerId).Select(e=>e.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int CountryId)
        {
            return _context.Owners.Where(s => s.Country.Id == CountryId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return Save();
        }
    }
}
