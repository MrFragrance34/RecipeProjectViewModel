using System;

namespace ClassLibrary4
{
    public class Review : BaseEntity
    {
        private Recipe recipe;
        private Cook cook;
        private int rating;
        private string commentText;
        private DateTime reviewDate;

        public Recipe Recipe { get => recipe; set => recipe = value; }
        public Cook Cook { get => cook; set => cook = value; }
        public int Rating { get => rating; set => rating = value; }
        public string CommentText { get => commentText; set => commentText = value; }
        public DateTime ReviewDate { get => reviewDate; set => reviewDate = value; }
        public Review()
        {
            reviewDate = DateTime.Now;
        }
        public override string ToString()
        {
            return $"ID: {Id}, Recipe ID: {Recipe?.Id}, Cook ID: {Cook?.Id}, " +
                   $"Rating: {Rating}, Comment: {CommentText}, Review Date: {ReviewDate}";
        }
    }
}