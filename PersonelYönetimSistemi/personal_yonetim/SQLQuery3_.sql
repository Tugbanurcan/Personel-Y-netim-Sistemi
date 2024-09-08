CREATE VIEW vwPersonelProje
AS
SELECT 
    p.personel_id,
    p.personel_ad,
    p.personel_soyad,
    p.personel_maas,
    d.departman_adý,
    pr.proje_adý
FROM 
    Personel p
INNER JOIN 
    Departman d ON p.departman_id = d.departman_id
LEFT JOIN 
    Proje pr ON p.personel_id = pr.personel_id;