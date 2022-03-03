#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CookbookApi.SqlExpress.Data;
using CookbookApi.SqlExpress.Model;

namespace CookbookApi.SqlExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly DataContext _context;

        public RecipesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            return await _context.Recipes.Include("Ingredients").ToListAsync();
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        // PUT: api/Recipes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return BadRequest();
            }
            var existingrecipe = await _context.Recipes.Include("Ingredients").FirstOrDefaultAsync(x=>x.Id == recipe.Id);
            if (recipe == null || existingrecipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(existingrecipe);
            try
            {
                _context.SaveChanges();
            }
            catch
            {

            }
            

            try
            {
                var newRecipe = new Recipe()
                {
                    Name = recipe.Name,
                    ImagePath = recipe.ImagePath,
                    Description = recipe.Description
                };
                _context.Recipes.Add(newRecipe);
                _context.SaveChanges();
                
                int recipeId = newRecipe.Id;
                List<Ingredient> ingredients = new List<Ingredient>();
                foreach(var ingredient in recipe.Ingredients)
                {
                    ingredients.Add(new Ingredient { Amount = ingredient.Amount, Name = ingredient.Name,RecipeId = recipeId });
                }
                _context.Ingredients.AddRange(ingredients);
                _context.SaveChanges();
                
            }
            catch
            {

            }
            

            return NoContent();
        }

        [HttpPost]
        [Route("AddUpdateRecipes")]
        public async Task<IActionResult> PostRecipes(IEnumerable<Recipe> recipes)
        {
            

            try
            {
                foreach (var recipe in recipes)
                {
                    if (recipe.Id == 0)
                    {
                        var newRecipe = new Recipe();
                        newRecipe.Name = recipe.Name;
                        newRecipe.Description = recipe.Description;
                        newRecipe.ImagePath = recipe.ImagePath;
                        newRecipe.Ingredients = recipe.Ingredients;
                        _context.Recipes.Add(newRecipe);
                    }
                    else
                    {
                        var existingRecipe =  _context.Recipes.Find(recipe.Id);
                        
                        _context.Recipes.Remove(existingRecipe);
                        var newRecipe = new Recipe
                        {
                            Name = recipe.Name,
                            Description = recipe.Description,
                            ImagePath = recipe.ImagePath,
                            Ingredients = recipe.Ingredients
                        };
                        _context.Recipes.Add(newRecipe);
                        
                       
                    }

                }
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return NoContent();
        }

        // POST: api/Recipes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe);
        }

        // DELETE: api/Recipes/5
        [HttpDelete]
        [Route("DeleteRecipeById/{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
