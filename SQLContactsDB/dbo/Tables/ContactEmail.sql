CREATE TABLE [dbo].[ContactEmail]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContactId] INT NOT NULL, 
    [EmailId] INT NOT NULL,
    FOREIGN KEY (ContactId) REFERENCES Contacts(Id),
    FOREIGN KEY (EmailId) REFERENCES EmailAddresses(Id),

)
