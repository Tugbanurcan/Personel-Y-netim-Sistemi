CREATE TABLE Personel (
    personel_id INT PRIMARY KEY IDENTITY(1,1),
    personel_ad NVARCHAR(50),
    personel_soyad NVARCHAR(50),
    personel_maas DECIMAL(18, 2),
    departman_id INT,
    FOREIGN KEY (departman_id) REFERENCES Departman(departman_id)
);