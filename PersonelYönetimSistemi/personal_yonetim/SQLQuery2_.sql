CREATE TRIGGER trg_InsertProjectOnPersonel
ON Personel
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Yeni eklenen personel i�in Proje tablosuna varsay�lan bir proje ekleyin
    INSERT INTO Proje (proje_ad�, personel_id)
    SELECT 'Default Project', inserted.personel_id
    FROM inserted;
END;