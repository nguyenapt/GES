using System.Collections.Generic;

namespace GES.Inside.Data.Configs
{
    public class Settings
    {
        public static int CacheExpiredAfterMin => 1440;

        public static string SdgUploadPath = "Sdg";

        public static IList<string> HiddenDocumentServices = new List<string> {"Oekom", "SDG", "EngTyp", "Clients > Reports > Annual","Clients > Reports > Quarterly"  };
    }
}

