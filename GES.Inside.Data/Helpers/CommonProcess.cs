using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Portfolio;

namespace GES.Inside.Data.Helpers
{
    public class CommonProcess
    {
        //TODO: MENDO
        //public static MsciSectorModel GetMsciSector(List<SubPeerGroups> mscis, int msciId)
        //{
        //    string sector = string.Empty, industry = string.Empty, industryGroup = string.Empty, sectorCode = string.Empty;

        //    var currentMsci = GetMsciById(mscis, msciId);

        //    if (currentMsci != null && currentMsci.Code.Length == 8)
        //    {
        //        currentMsci = GetMsciById(mscis, currentMsci.ParentI_Msci_Id);
        //    }

        //    if (currentMsci != null && currentMsci.Code.Length == 6)
        //    {
        //        industry = currentMsci.Name;
        //        currentMsci = GetMsciById(mscis, currentMsci.ParentI_Msci_Id);
        //    }

        //    if (currentMsci != null && currentMsci.Code.Length == 4)
        //    {
        //        industryGroup = currentMsci.Name;
        //        currentMsci = GetMsciById(mscis, currentMsci.ParentI_Msci_Id);
        //    }

        //    if (currentMsci != null && currentMsci.Code.Length == 2)
        //    {
        //        sector = currentMsci.Name;
        //        sectorCode = currentMsci.Code;
        //    }

        //    return new MsciSectorModel { MsciId = msciId, Industry = industry, IndustryGroup = industryGroup, Sector = sector, SectorCode = sectorCode };
        //}

        //public static SubPeerGroups GetMsciById(List<SubPeerGroups> mscis, int? msciId)
        //{
        //    if (msciId == null) return null;
        //    return mscis.FirstOrDefault(d => d.Id == msciId);
        //}

    }
}
