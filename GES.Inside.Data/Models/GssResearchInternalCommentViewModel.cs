using System;

namespace GES.Inside.Data.Models
{
    public class GssResearchInternalCommentViewModel
    {
        public Guid Id { get; set; }

        public Guid GssId { get; set; }

        public DateTime? DateModified { get; set; }

        public int Level { get; set; }

        public long User_Id { get; set; }

        public string UserName { get; set; }

        public string Comment { get; set; }

    }
}
