using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public class SqlCrud
    {
        private string _connectionString;
        private SqlDataAccess db = new SqlDataAccess();

        public SqlCrud(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<BasicContactModel> GetAllContacts()
        {
            string sql = "select Id, FirstName, LastName from dbo.Contacts";

            return db.LoadData<BasicContactModel, dynamic>(sql, new { }, _connectionString);
        }

        public FullContactModel GetFullContactById(int id)
        {
            string sql = "select Id, FirstName, LastName from dbo.Contacts where Id = @Id";
            FullContactModel output = new();

            output.BasicInfo = db.LoadData<BasicContactModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

            if (output.BasicInfo == null)
            {
                // do something to tell the user that the record was not found
                throw new Exception("User not found");
                return null;
            }

            sql = @"select e.*
                from dbo.EmailAddresses e
                join dbo.ContactEmail ce on ce.EmailId = e.Id
                where ce.ContactId = @Id";

            output.EmailAddresses = db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = id }, _connectionString);

            sql = "select pn.* " +
                "from dbo.PhoneNumbers pn " +
                "join dbo.ContactPhoneNumber cpn on cpn.PhoneNumberId = pn.Id " +
                "where cpn.ContactId = @Id ";

            output.PhoneNumbers = db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = id }, _connectionString);

            return output;

        }

        public void CreateContact(FullContactModel contact)
        {
            string sql = "insert into dbo.Contacts (FirstName, LastName) values (@FirstName, @LastName);";
            // Save the basic contact
            db.SaveData(sql,
                new { FirstName = contact.BasicInfo.FirstName, LastName = contact.BasicInfo.LastName },
                _connectionString);

            // Get ID number of the contact
            sql = "select Id from dbo.Contacts " +
                "where FirstName = @Firstname " +
                "and LastName = @LastName;";
            int contactId = db.LoadData<IdLookupModel, dynamic>(
                sql,
                new { FirstName = contact.BasicInfo.FirstName, LastName = contact.BasicInfo.LastName },
                _connectionString).First().Id;

            foreach (var phoneNumber in contact.PhoneNumbers)
            {

                if (phoneNumber.Id == 0)
                {
                    sql = "insert into dbo.PhoneNumbers (PhoneNumber) values (@PhoneNumber);";
                    db.SaveData(sql, new {phoneNumber.PhoneNumber}, _connectionString);


                    sql = "select Id from dbo.PhoneNumbers " +
                        "where PhoneNumber = @PhoneNumber;";
                    phoneNumber.Id = db.LoadData<IdLookupModel, dynamic>(
                        sql,
                        new { phoneNumber.PhoneNumber },
                        _connectionString).First().Id;
                }

                sql = "insert into dbo.ContactPhoneNumber (ContactId, PhoneNumberId) " +
                    "values (@ContactId, @PhoneNumberId)";
                db.SaveData(sql, new { ContactId = contactId, PhoneNumberId = phoneNumber.Id}, _connectionString);
            }

            // Do the same for email

            foreach (var email in contact.EmailAddresses)
            {
                if (email.Id == 0)
                {
                    sql = "insert into dbo.EmailAddresses (EmailAddress) values (@EmailAddress);";
                    db.SaveData(sql, new { email.EmailAddress }, _connectionString);

                    sql = "select Id " +
                        "from dbo.EmailAddresses " +
                        "where EmailAddress = @EmailAddress;";
                    email.Id = db.LoadData<IdLookupModel, dynamic>(
                        sql,
                        new { email.EmailAddress },
                        _connectionString).First().Id;
                }

                sql = "insert into dbo.ContactEmail (ContactId, EmailId) " +
                    "values (@ContactId, @EmailId)";
                db.SaveData(sql, new { ContactId = contactId, EmailId = email.Id }, _connectionString);
            }
        }
    }
}
