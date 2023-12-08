using PokemonReviewApp.Models;

namespace PokemonReviewApp.interfaces
{
    public interface ICountryReposotry
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int OwnerId);
        ICollection<Owner> GetOwnersFromCountry(int CountryId);
        bool CountryExsits(int id);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool Save();
    }
}
