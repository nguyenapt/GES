using System;

namespace GES.Inside.Data.Models
{
    public class Glossary
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }
    }
}