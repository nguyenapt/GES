CREATE VIEW screeningreportview 
AS 
  SELECT c.id 
         AS 
            SustainalyticsID, 
         c.isin 
         AS 
            ISIN, 
         c.NAME 
         AS 
            CompanyName, 
         rp.reportincident 
         AS 
            Issue, 
         rp.i_gescasereports_id, 
         n.NAME 
         AS 
            Involvement, 
         co.NAME 
         AS 
            Location, 
         COALESCE(dbo.Formatdatetime(rp.entrydate, 'YYYY-MM-DD'), '') 
         AS 
            EntryDate, 
         st.NAME 
         AS 
            NewEngagementStatus, 
         dbo.Getrecommendationnamebefore(rp.i_gescasereports_id) 
         AS 
         [Previous Engagement status], 
         COALESCE(dbo.Formatdatetime( 
                  dbo.Getengagementsince(rp.i_gescasereports_id), 
                  'YYYY-MM-DD'), '') 
         AS 
            [Engagement since], 
         CASE 
           WHEN rp.newi_gescasereportstatuses_id IN ( 7 ) THEN 
           rp.processgoal + ' - ' 
           + COALESCE(dbo.Formatdatetime(rp.processgoalmodified, 'YYYY-MM-DD'), 
           '' 
           ) 
           ELSE '' 
         END 
         AS 
            [Change Objective], 
         CASE 
           WHEN rp.newi_gescasereportstatuses_id IN ( 7 ) THEN 
           (SELECT COALESCE(dbo.Formatdatetime(Max(contactdate), 'YYYY-MM-DD'), 
                   '' 
                   ) 
            FROM   (SELECT i_gescompanydialogues.contactdate 
                    FROM   i_gescompanydialogues 
                    WHERE  i_gescompanydialogues.i_gescasereports_id = 
                           rp.i_gescasereports_id 
                           AND i_gescompanydialogues.classa = 1 
                    UNION ALL 
                    SELECT i_gessourcedialogues.contactdate 
                    FROM   i_gessourcedialogues 
                    WHERE  i_gessourcedialogues.i_gescasereports_id = 
                           rp.i_gescasereports_id 
                           AND i_gessourcedialogues.classa = 1) sq) 
           ELSE '' 
         END 
         AS 
            [Engagement Activity], 
         resp.shortname 
         AS 
            [Engagement Response], 
         prg.shortname 
         AS 
            [Engagement Progress], 
         dbo.Fncalcdevelopmentgrade(prg.i_progressstatuses_id, 
         resp.i_responsestatuses_id) 
         AS 
            [Engagement Performance], 
         COALESCE(dbo.Formatdatetime(mil.created, 'YYYY-MM-DD'), '') 
         AS 
            [Milestone date], 
         mil.description 
         AS 
            [Milestone achieved], 
         COALESCE(dbo.Formatdatetime(com.commentarymodified, 'YYYY-MM-DD'), '') 
         AS 
         [Commentary date], 
         com.description 
         AS 
            Commentary 
  FROM   i_companies c 
         JOIN i_gescompanies gc 
           ON c.i_companies_id = gc.i_companies_id 
         JOIN i_gescasereports rp 
           ON gc.i_gescompanies_id = rp.i_gescompanies_id 
         JOIN countries co 
           ON rp.countryid = co.id 
         LEFT JOIN i_normareas n 
                ON rp.i_normareas_id = n.i_normareas_id 
         LEFT JOIN i_gescasereportstatuses st 
                ON rp.newi_gescasereportstatuses_id = 
                   st.i_gescasereportstatuses_id 
         LEFT JOIN i_responsestatuses resp 
                ON resp.i_responsestatuses_id = rp.i_responsestatuses_id 
         LEFT JOIN i_progressstatuses prg 
                ON prg.i_progressstatuses_id = rp.i_progressstatuses_id 
         LEFT JOIN i_milestones mil 
                ON rp.i_gescasereports_id = mil.i_gescasereports_id 
         LEFT JOIN i_gescommentary com 
                ON rp.i_gescasereports_id = com.i_gescasereports_id 