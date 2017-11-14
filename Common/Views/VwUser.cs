
using System;

namespace Common.Views
{
    public class VwUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public int Phone { get; set; }
        public int GenderID { get; set; }
        public int TownID { get; set; }
        public bool Blocked { get; set; }
        public int NoOfAttempts { get; set; }
        public int AddressType { get; set; }
        public string Street { get; set; }
        public int ContactType { get; set; }
        public int Mobile { get; set; }
        public int? UserType { get; set; }
        public int? AddressID { get; set; }
        public int? ContactID { get; set; }
        public string Town { get; set; }
    }
}
