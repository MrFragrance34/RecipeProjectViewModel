
using System;
using ClassLibrary4;
using ViewModel;

namespace Test2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            

            string suffix = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");

            try
            {
                
                UsersDB usersDB = new UsersDB();
                AdminsDB adminsDB = new AdminsDB();
                CookDB cookDB = new CookDB();
                CategoryDB categoryDB = new CategoryDB();
                RecipesDB recipesDB = new RecipesDB();
                ReviewsDB reviewsDB = new ReviewsDB();
                RecipeCategoryDB recipeCategoryDB = new RecipeCategoryDB();

               

                Console.WriteLine("====================================");
                Console.WriteLine("       SELECT ALL TESTS");
                Console.WriteLine("====================================");

                Console.WriteLine("Users: " + usersDB.SelectAll().Count);
                Console.WriteLine("Admins: " + adminsDB.SelectAll().Count);
                Console.WriteLine("Cooks: " + cookDB.SelectAll().Count);
                Console.WriteLine("Categories: " + categoryDB.SelectAll().Count);
                Console.WriteLine("Recipes: " + recipesDB.SelectAll().Count);
                Console.WriteLine("Reviews: " + reviewsDB.SelectAll().Count);
                Console.WriteLine("Recipe categories: " + recipeCategoryDB.SelectAll().Count);

                

                Console.WriteLine("\n====================================");
                Console.WriteLine("       USERS CRUD TEST");
                Console.WriteLine("====================================");

                User newUser = new User()
                {
                    DisplayName = "DB Test User " + suffix,
                    Email = "user_" + suffix + "@test.com",
                    PasswordHash = "123456",
                    JoinDate = DateTime.Now
                };

                usersDB.Insert(newUser);
                Console.WriteLine("Users INSERT: " + usersDB.SaveChanges() + " row(s)");

                User testUser = FindUserByEmail(usersDB, newUser.Email);

                if (testUser == null)
                    throw new Exception("Test user was not found after INSERT.");

                testUser.DisplayName = "Updated User " + suffix;

                usersDB.Update(testUser);
                Console.WriteLine("Users UPDATE: " + usersDB.SaveChanges() + " row(s)");

                testUser = FindUserByEmail(usersDB, newUser.Email);

                Console.WriteLine("Users SELECT after UPDATE: " +
                    (testUser != null ? testUser.DisplayName : "Not found"));

                

                Console.WriteLine("\n====================================");
                Console.WriteLine("       COOKS CRUD TEST");
                Console.WriteLine("====================================");

                Cook newCook = new Cook()
                {
                    Id = testUser.Id,
                    Bio = "Test Cook Bio " + suffix,
                    ProfilePicPath = "test_cook.jpg"
                };

                cookDB.Insert(newCook);
                Console.WriteLine("Cooks INSERT: " + cookDB.SaveChanges() + " row(s)");

                Cook testCook = FindCookById(cookDB, testUser.Id);

                if (testCook == null)
                    throw new Exception("Test cook was not found after INSERT.");

                testCook.Bio = "Updated Cook Bio " + suffix;

                cookDB.Update(testCook);
                Console.WriteLine("Cooks UPDATE: " + cookDB.SaveChanges() + " row(s)");

                testCook = FindCookById(cookDB, testUser.Id);

                Console.WriteLine("Cooks SELECT after UPDATE: " +
                    (testCook != null ? testCook.Bio : "Not found"));

                

                Console.WriteLine("\n====================================");
                Console.WriteLine("       CATEGORIES CRUD TEST");
                Console.WriteLine("====================================");

                Category newCategory = new Category()
                {
                    CategoryName = "Test Category " + suffix
                };

                categoryDB.Insert(newCategory);
                Console.WriteLine("Categories INSERT: " +
                    categoryDB.SaveChanges() + " row(s)");

                Category testCategory =
                    FindCategoryByName(categoryDB, newCategory.CategoryName);

                if (testCategory == null)
                    throw new Exception("Test category was not found after INSERT.");

                testCategory.CategoryName = "Updated Category " + suffix;

                categoryDB.Update(testCategory);
                Console.WriteLine("Categories UPDATE: " +
                    categoryDB.SaveChanges() + " row(s)");

                testCategory =
                    FindCategoryByName(categoryDB, "Updated Category " + suffix);

                Console.WriteLine("Categories SELECT after UPDATE: " +
                    (testCategory != null ? testCategory.CategoryName : "Not found"));

                

                Console.WriteLine("\n====================================");
                Console.WriteLine("       RECIPES CRUD TEST");
                Console.WriteLine("====================================");

                Recipe newRecipe = new Recipe()
                {
                    Title = "Test Recipe " + suffix,
                    Description = "Test recipe description",
                    Instructions = "Mix all ingredients and cook.",
                    PrepTimeMins = 30,
                    ImagePath = "test_recipe.jpg",
                    Cook = testCook
                };

                recipesDB.Insert(newRecipe);
                Console.WriteLine("Recipes INSERT: " +
                    recipesDB.SaveChanges() + " row(s)");

                Recipe testRecipe =
                    FindRecipeByTitle(recipesDB, newRecipe.Title);

                if (testRecipe == null)
                    throw new Exception("Test recipe was not found after INSERT.");

                testRecipe.Title = "Updated Recipe " + suffix;
                testRecipe.Description = "Updated recipe description";
                testRecipe.PrepTimeMins = 45;

                recipesDB.Update(testRecipe);
                Console.WriteLine("Recipes UPDATE: " +
                    recipesDB.SaveChanges() + " row(s)");

                testRecipe =
                    FindRecipeByTitle(recipesDB, "Updated Recipe " + suffix);

                Console.WriteLine("Recipes SELECT after UPDATE: " +
                    (testRecipe != null ? testRecipe.Title : "Not found"));

                Console.WriteLine("\n====================================");
                Console.WriteLine("   RECIPE-CATEGORY CRUD TEST");
                Console.WriteLine("====================================");

                RecipeCategory newRecipeCategory = new RecipeCategory()
                {
                    Recipe = testRecipe,
                    Category = testCategory
                };

                recipeCategoryDB.Insert(newRecipeCategory);
                Console.WriteLine("RecipeCategory INSERT: " +
                    recipeCategoryDB.SaveChanges() + " row(s)");

                RecipeCategory testRecipeCategory = recipeCategoryDB.SelectAll()
                    .Find(item => item.Recipe != null && item.Category != null
                        && item.Recipe.Id == testRecipe.Id
                        && item.Category.Id == testCategory.Id);

                if (testRecipeCategory == null)
                    throw new Exception("Recipe category was not found after INSERT.");

                // Update the same foreign keys to verify the UPDATE statement.
                testRecipeCategory.Recipe = testRecipe;
                testRecipeCategory.Category = testCategory;
                recipeCategoryDB.Update(testRecipeCategory);
                Console.WriteLine("RecipeCategory UPDATE: " +
                    recipeCategoryDB.SaveChanges() + " row(s)");

                Console.WriteLine("\n====================================");
                Console.WriteLine("       REVIEWS CRUD TEST");
                Console.WriteLine("====================================");

                Review newReview = new Review()
                {
                    Recipe = testRecipe,
                    Cook = testCook,
                    Rating = 4,
                    CommentText = "Test Review " + suffix
                };

                reviewsDB.Insert(newReview);
                Console.WriteLine("Reviews INSERT: " +
                    reviewsDB.SaveChanges() + " row(s)");

                Review testReview =
                    FindReviewByComment(reviewsDB, newReview.CommentText);

                if (testReview == null)
                    throw new Exception("Test review was not found after INSERT.");

                testReview.Rating = 5;
                testReview.CommentText = "Updated Review " + suffix;

                reviewsDB.Update(testReview);
                Console.WriteLine("Reviews UPDATE: " +
                    reviewsDB.SaveChanges() + " row(s)");

                testReview =
                    FindReviewByComment(reviewsDB, "Updated Review " + suffix);

                Console.WriteLine("Reviews SELECT after UPDATE: " +
                    (testReview != null ? testReview.CommentText : "Not found"));

                

                Console.WriteLine("\n====================================");
                Console.WriteLine("       ADMINS CRUD TEST");
                Console.WriteLine("====================================");

                Admin newAdmin = new Admin()
                {
                    DisplayName = "Test Admin " + suffix,
                    Email = "admin_" + suffix + "@test.com",
                    PasswordHash = "123456",
                    JoinDate = DateTime.Now,
                    AdminRole = "Moderator"
                };

                adminsDB.Insert(newAdmin);
                Console.WriteLine("Admins INSERT: " +
                    adminsDB.SaveChanges() + " row(s)");

                Admin testAdmin =
                    FindAdminByEmail(adminsDB, newAdmin.Email);

                if (testAdmin == null)
                    throw new Exception("Test admin was not found after INSERT.");

                testAdmin.DisplayName = "Updated Admin " + suffix;
                testAdmin.AdminRole = "Administrator";

                adminsDB.Update(testAdmin);
                Console.WriteLine("Admins UPDATE: " +
                    adminsDB.SaveChanges() + " row(s)");

                testAdmin = FindAdminByEmail(adminsDB, newAdmin.Email);

                Console.WriteLine("Admins SELECT after UPDATE: " +
                    (testAdmin != null ? testAdmin.DisplayName : "Not found"));

               

                Console.WriteLine("\n====================================");
                Console.WriteLine("       DELETE TESTS");
                Console.WriteLine("====================================");

                reviewsDB.Delete(testReview);
                Console.WriteLine("Reviews DELETE: " +
                    reviewsDB.SaveChanges() + " row(s)");

                recipeCategoryDB.Delete(testRecipeCategory);
                Console.WriteLine("RecipeCategory DELETE: " +
                    recipeCategoryDB.SaveChanges() + " row(s)");

                recipesDB.Delete(testRecipe);
                Console.WriteLine("Recipes DELETE: " +
                    recipesDB.SaveChanges() + " row(s)");

                categoryDB.Delete(testCategory);
                Console.WriteLine("Categories DELETE: " +
                    categoryDB.SaveChanges() + " row(s)");

                cookDB.Delete(testCook);
                Console.WriteLine("Cooks DELETE: " +
                    cookDB.SaveChanges() + " row(s)");

                usersDB.Delete(testUser);
                Console.WriteLine("Users DELETE: " +
                    usersDB.SaveChanges() + " row(s)");

                adminsDB.Delete(testAdmin);
                Console.WriteLine("Admins DELETE: " +
                    adminsDB.SaveChanges() + " row(s)");

                

                Console.WriteLine("\n====================================");
                Console.WriteLine("       FINAL DATABASE CHECK");
                Console.WriteLine("====================================");

                Console.WriteLine("Users: " + usersDB.SelectAll().Count);
                Console.WriteLine("Admins: " + adminsDB.SelectAll().Count);
                Console.WriteLine("Cooks: " + cookDB.SelectAll().Count);
                Console.WriteLine("Categories: " + categoryDB.SelectAll().Count);
                Console.WriteLine("Recipes: " + recipesDB.SelectAll().Count);
                Console.WriteLine("Reviews: " + reviewsDB.SelectAll().Count);
                Console.WriteLine("Recipe categories: " + recipeCategoryDB.SelectAll().Count);

                Console.WriteLine("\nALL TESTS COMPLETED!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nTEST FAILED!");
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        

        static User FindUserByEmail(UsersDB db, string email)
        {
            foreach (User user in db.SelectAll())
            {
                if (user.Email == email)
                    return user;
            }

            return null;
        }

        static Cook FindCookById(CookDB db, int id)
        {
            foreach (Cook cook in db.SelectAll())
            {
                if (cook.Id == id)
                    return cook;
            }

            return null;
        }

        static Category FindCategoryByName(CategoryDB db, string name)
        {
            foreach (Category category in db.SelectAll())
            {
                if (category.CategoryName == name)
                    return category;
            }

            return null;
        }

        static Recipe FindRecipeByTitle(RecipesDB db, string title)
        {
            foreach (Recipe recipe in db.SelectAll())
            {
                if (recipe.Title == title)
                    return recipe;
            }

            return null;
        }

        static Review FindReviewByComment(ReviewsDB db, string comment)
        {
            foreach (Review review in db.SelectAll())
            {
                if (review.CommentText == comment)
                    return review;
            }

            return null;
        }

        static Admin FindAdminByEmail(AdminsDB db, string email)
        {
            foreach (Admin admin in db.SelectAll())
            {
                if (admin.Email == email)
                    return admin;
            }

            return null;
        }
    }
}
