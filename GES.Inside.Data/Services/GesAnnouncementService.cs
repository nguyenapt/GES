using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Syndication;

namespace GES.Inside.Data.Services
{
    public class GesAnnouncementService : EntityService<GesEntities, GesAnnouncement>, I_GesAnnouncementService
    {
        private readonly GesEntities _dbContext;
        private readonly I_GesAnnouncementRepository _gesAnnouncementRepository;

        public GesAnnouncementService(IUnitOfWork<GesEntities> unitOfWork, I_GesAnnouncementRepository gesAnnouncementRepository, IGesLogger logger)
            : base(unitOfWork, logger, gesAnnouncementRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gesAnnouncementRepository = gesAnnouncementRepository;
        }

        public GesAnnouncement GetById(Guid id)
        {
            return this.SafeExecute<GesAnnouncement>(() => _gesAnnouncementRepository.FindBy(x => x.GesAnnouncementId == id).FirstOrDefault());
        }

        public IList<GesAnnouncementModel> GetModels()
        {
            var result = from an in _dbContext.GesAnnouncement                         
                         select new GesAnnouncementModel { AnnouncementDate = an.AnnouncementDate,Content = an.Content, GesAnnouncementId = an.GesAnnouncementId,LinkTitle = an.LinkTitle,Title = an.Title};

            return result.OrderByDescending(d => d.AnnouncementDate).ToList();
        }

        public IList<GesAnnouncementModel> GetRssModels(string url)
        {
            var lstRssItems = new List<GesAnnouncementModel>();
            if (!string.IsNullOrEmpty(url))
            {
                XmlReader reader = XmlReader.Create(url);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                reader.Close();
                foreach (SyndicationItem item in feed.Items)
                {
                    var model = new GesAnnouncementModel();
                    model.Title = item.Title.Text;
                    model.LinkTitle = item.Links.FirstOrDefault()?.Uri.ToString();
                    model.Content = item.Summary.Text;
                    model.AnnouncementDate = item.PublishDate.UtcDateTime;
                    lstRssItems.Add(model);
                }
            }

            return lstRssItems;
        }
    }
}
