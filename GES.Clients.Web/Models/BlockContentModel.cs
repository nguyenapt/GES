using System;

namespace GES.Clients.Web.Models
{
    public class BlockContentModel
    {
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public string CustomContent { get; set; }
        public string PopupModalTitle { get; set; }
    }
}