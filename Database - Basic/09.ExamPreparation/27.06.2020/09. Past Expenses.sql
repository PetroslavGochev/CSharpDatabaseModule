SELECT j.JobId,
       ISNULL(SUM(P.Price * Op.Quantity), 0.00) AS Total
    FROM Jobs AS j
             LEFT JOIN Orders O
                       ON j.JobId = O.JobId
             LEFT JOIN OrderParts Op
                       ON O.OrderId = Op.OrderId
             LEFT JOIN Parts P
                       ON P.PartId = Op.PartId
    WHERE j.Status = 'Finished'
    GROUP BY j.JobId
    ORDER BY Total DESC,
             j.JobId