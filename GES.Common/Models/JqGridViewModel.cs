
namespace GES.Common.Models
{
    public class JqGridViewModel
    {
        public string sidx { get; set; }
        public string sord { get; set; }
        public int page { get; set; }
        public int rows { get; set; }
        public bool _search { get; set; }
        public string searchOper { get; set; }
        public string searchString { get; set; }
        public string searchField { get; set; }
        public string filters { get; set; }
    }
}