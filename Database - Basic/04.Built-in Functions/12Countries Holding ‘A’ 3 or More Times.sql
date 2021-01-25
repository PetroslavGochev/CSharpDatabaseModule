--12.Countries Holding ‘A’ 3 or More Times
SELECT [CountryName],[IsoCode] FROM [dbo].[Countries]
WHERE LEN([CountryName]) - 2 > LEN(REPLACE([CountryName],'A',''))
ORDER BY [IsoCode]
