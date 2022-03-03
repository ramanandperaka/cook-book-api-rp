namespace CookbookApi.SqlExpress.Model
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string ImagePath { get; set; }

        //Navigation properties
        public ICollection<Ingredient>? Ingredients { get; set; }
    }
}
