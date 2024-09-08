CREATE TRIGGER trg_InsertProjectOnPersonel
ON Personel
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Yeni eklenen personel için Proje tablosuna varsayýlan bir proje ekleyin
    INSERT INTO Proje (proje_adý, personel_id)
    SELECT 'Default Project', inserted.personel_id
    FROM inserted;
END;