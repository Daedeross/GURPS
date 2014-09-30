CREATE FUNCTION [dbo].[udfGetThreshold]
(
	@maxVal int,
	@curVal int
)
RETURNS int
AS
BEGIN
    DECLARE @ret int;
	DECLARE @thresh float;
	SET @thresh = (@maxVal - @curVal) / CONVERT (float, @maxVal);
	SET @ret = CASE
		WHEN @thresh > 0.3333 THEN 2
		WHEN @thresh > 0 THEN 1
		ELSE CEILING(@thresh)
	END;
	RETURN @ret;
END