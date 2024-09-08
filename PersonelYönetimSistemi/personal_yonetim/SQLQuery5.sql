CREATE PROCEDURE AddDepartman
    @departmanAdi NVARCHAR(50)
AS
BEGIN
    INSERT INTO Departman (departman_adý)
    VALUES (@departmanAdi)
END
