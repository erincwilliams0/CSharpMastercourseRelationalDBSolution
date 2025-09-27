CREATE TABLE [dbo].[ContactPhoneNumber]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContactId] INT NOT NULL, 
    [PhoneNumberId] INT NOT NULL,
    FOREIGN KEY (ContactId) REFERENCES Contacts(Id),
    FOREIGN KEY (PhoneNumberId) REFERENCES PhoneNumbers(Id)
)
