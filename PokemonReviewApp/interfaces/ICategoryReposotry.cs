using PokemonReviewApp.Models;

namespace PokemonReviewApp.interfaces
{
    public interface ICategoryReposotry
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonByCategory(int categoryId);
        bool CategoryExsits(int id);
        // post
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);

        bool Save();
    }
}
