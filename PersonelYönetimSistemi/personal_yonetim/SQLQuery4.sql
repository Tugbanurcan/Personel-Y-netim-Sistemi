CREATE TABLE Proje (
    proje_id INT PRIMARY KEY IDENTITY(1,1),
    proje_adý NVARCHAR(100),
    personel_id INT,
    FOREIGN KEY (personel_id) REFERENCES Personel(personel_id)
);