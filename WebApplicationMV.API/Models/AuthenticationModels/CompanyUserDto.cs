namespace WebApplicationMV.API.Models.AuthenticationModels
{
    public class CompanyUserDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }

        public CompanyUserDto(string userName, string firstName, string lastName, string company)
        {            
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Company = company;
        }
    }
}
