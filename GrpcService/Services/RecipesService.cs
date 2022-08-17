﻿using Grpc.Core;
using GrpcServer.Protos;
namespace GrpcServer.Services
{
    public class RecipesService : Recipes.RecipesBase
    {
        private readonly ILogger<Recipe> _logger;
        private readonly Data data;

        public RecipesService(ILogger<Recipe> logger)
        {
            _logger = logger;
            data = Data.GetInstance(logger);
        }

        public override async Task<RecipeList> GetAllRecipes(EmptyRequest request, ServerCallContext context)
        {
            var recipes = await data.GetRecipesAsync();
            RecipeList recipeList = new();
            foreach(var recipe in recipes)
            {
                recipeList.Recipes.Add(recipe);
            }
            return recipeList;
        }

        public override async Task<Recipe> GetRecipe(RecipeRequest request, ServerCallContext context)
        {
            var recipe = await data.GetRecipeAsync(new Guid(request.Id));
            return recipe;
        }

        public override async Task<RecipeResponse> AddRecipe(Recipe recipe, ServerCallContext context)
        {
            await data.AddRecipeAsync(recipe);
            return new RecipeResponse() { Recipe = recipe, StatusCode = StatusCodes.Status200OK };
        }

        public override async Task<RecipeResponse> EditRecipe(EditRecipeRequest request, ServerCallContext context)
        {
            var updatedRecipe = await data.EditRecipeAsync(new Guid(request.Id), request.NewRecipe);
            return new RecipeResponse() { StatusCode = StatusCodes.Status200OK, Recipe = updatedRecipe };
        }

        public override async Task<RecipeResponse> DeleteRecipe(RecipeRequest request, ServerCallContext context)
        {
            await data.RemoveRecipeAsync(new Guid(request.Id));
            return new RecipeResponse(){ StatusCode = StatusCodes.Status200OK, Recipe = null};
        }

        public override async Task<CategoryList> GetAllCategories(EmptyRequest request, ServerCallContext context)
        {
            var categories = await data.GetAllCategoriesAsync();
            CategoryList categoryList = new();
            foreach (var category in categories)
            {
                categoryList.Categories.Add(category);
            }
            return categoryList;
        }

        public override async Task<CategoryResponse> AddCategory(Category request, ServerCallContext context)
        {
            await data.AddCategoryAsync(request.CategoryName);
            return new CategoryResponse() { StatusCode = 200 };
        }

        public override async Task<CategoryResponse> EditCategory(EditCategoryRequest request, ServerCallContext context)
        {
            await data.EditCategoryAsync(request.OldCategory,request.NewCategory);
            return new CategoryResponse() { StatusCode = 200 };
        }

        public override async Task<CategoryResponse> DeleteCategory(Category request, ServerCallContext context)
        {
            await data.RemoveCategoryAsync(request.CategoryName);
            return new CategoryResponse() { StatusCode = 200 };
        }
    }
}
