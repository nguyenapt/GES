UPDATE dbo.G_Individuals
SET Email = REPLACE(Email, '@gesinternational.com', '@sustainalytics.com')
WHERE Email LIKE '%@gesinternational.com%'

UPDATE dbo.G_Individuals
SET Email = REPLACE(Email, '@ges-invest.com', '@sustainalytics.com')
WHERE Email LIKE '%@ges-invest.com%'

UPDATE ges.Users
SET Email = REPLACE(Email, '@gesinternational.com', '@sustainalytics.com')
WHERE Email LIKE '%@gesinternational.com%'

UPDATE ges.Users
SET Email = REPLACE(Email, '@ges-invest.com', '@sustainalytics.com')
WHERE Email LIKE '%@ges-invest.com%'
