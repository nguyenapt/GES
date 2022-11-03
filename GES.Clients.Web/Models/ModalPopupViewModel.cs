using System.Collections.Generic;
using GES.Inside.Data.Models.Anonymous;

namespace GES.Clients.Web.Models
{
    public class ModalPopupViewModel
    {
        public IList<KeyValueObject<string, string>> DateContentPairs;
        public IList<KeyValueAttributeObject<string, string, string>> DateContentWithAttributePairs;
        public string ModalTitle { get; set; }
    }
}