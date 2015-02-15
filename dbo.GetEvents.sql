CREATE PROCEDURE [dbo].[GetEvents]
	@date date, 
    @day int,	
    @month int,
    @dayOfWeek int,
    @weekOfMonth int
AS
SELECT * 
FROM(
	SELECT EV.*, ET.StartDate, ET.EndDate --one-time events
	FROM Events EV
	RIGHT JOIN EventTimeDatas ET ON ET.EventId = EV.Id
	WHERE (ET.RepeatInterval IS NULL AND ET.RepeatDay IS NULL AND  ET.RepeatDayOfWeek IS NULL 
	AND ET.RepeatMonth IS NULL AND ET.RepeatWeekOfMonth IS NULL)
	UNION
	SELECT EV.*, ET.StartDate, ET.EndDate --recouring events - simple
	FROM Events EV
	RIGHT JOIN EventTimeDatas ET ON ET.EventId = EV.Id
	WHERE (DATEDIFF(day, @date, CONVERT(date, ET.StartDate))%ET.RepeatInterval = 0)
	UNION
	SELECT EV.*, ET.StartDate, ET.EndDate --recouring events - complex real date
	FROM Events EV
	RIGHT JOIN EventTimeDatas ET ON ET.EventId = EV.Id
	WHERE (ET.RepeatDay = @day AND ET.RepeatMonth = @month) 
		OR (ET.RepeatDay = @day AND ET.RepeatMonth IS NULL) 
		OR (ET.RepeatMonth = @month AND ET.RepeatDay IS NULL)
	UNION
	SELECT EV.*, ET.StartDate, ET.EndDate --recouring events - complex day of week/week of month
	FROM Events EV
	RIGHT JOIN EventTimeDatas ET ON ET.EventId = EV.Id
	WHERE (ET.RepeatDayOfWeek = @dayOfWeek AND ET.RepeatWeekOfMonth = @weekOfMonth AND ET.RepeatMonth = @month) 
		OR (ET.RepeatDayOfWeek = @dayOfWeek AND ET.RepeatWeekOfMonth IS NULL AND ET.RepeatMonth IS NULL)
		OR (ET.RepeatDayOfWeek IS NULL AND ET.RepeatWeekOfMonth = @weekOfMonth AND ET.RepeatMonth IS NULL)
		OR (ET.RepeatDayOfWeek IS NULL AND ET.RepeatWeekOfMonth IS NULL AND ET.RepeatMonth = @month)
		OR (ET.RepeatDayOfWeek IS NULL AND ET.RepeatWeekOfMonth = @weekOfMonth AND ET.RepeatMonth = @month)
		OR (ET.RepeatDayOfWeek = @dayOfWeek AND ET.RepeatWeekOfMonth IS NULL AND ET.RepeatMonth = @month)
		OR(ET.RepeatDayOfWeek = @dayOfWeek AND ET.RepeatWeekOfMonth = @weekOfMonth AND ET.RepeatMonth IS NULL)) AS E
WHERE CONVERT(date, E.StartDate) <= @date AND (E.EndDate IS NULL OR CONVERT(date, E.EndDate) >= @date)
RETURN 0
