namespace PokemonReviewApp.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //one to many relationship
        public ICollection<Owner> Owners { get; set; }
    }
}
