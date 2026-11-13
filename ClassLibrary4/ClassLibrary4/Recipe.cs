using System;

namespace ClassLibrary4
{
    public class Recipe : BaseEntity
    {
        private string title;
        private string description;
        private string instructions;
        private int prepTimeMins;
        private string imagePath;
        private Cook cook;
        private DateTime recipeDate;

        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public string Instructions { get => instructions; set => instructions = value; }
        public int PrepTimeMins { get => prepTimeMins; set => prepTimeMins = value; }
        public string ImagePath { get => imagePath; set => imagePath = value; }
        public Cook Cook { get => cook; set => cook = value; }
        public DateTime RecipeDate { get => recipeDate; set => recipeDate = value; }

        public Recipe()
        { recipeDate = DateTime.Now; }
        public override string ToString()
        {
            return $"ID: {Id}, Title: {Title}, Description: {Description}, " +
                   $"Instructions: {Instructions}, Prep Time: {PrepTimeMins}, " +
                   $"Image: {ImagePath}, Cook ID: {Cook?.Id}, Recipe Date: {RecipeDate}";
        }

    }
}