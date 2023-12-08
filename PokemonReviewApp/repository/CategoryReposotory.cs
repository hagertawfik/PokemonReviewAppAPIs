using PokemonReviewApp.Data;
using PokemonReviewApp.interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.repository
{
    public class CategoryReposotory : ICategoryReposotry
    {
        private  DataContext _context;
        public CategoryReposotory( DataContext context) 
        {
            _context = context;
        
        }
        public bool CategoryExsits(int id)
        {
           return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(s=>s.Id==id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategeroies.Where(s => s.CategeroyId == categoryId).Select(e=>e.Pokemon).ToList();
        }

        public bool Save()
        {
           var saved = _context.SaveChanges();
            return saved > 0 ? true:false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }
    }
}
