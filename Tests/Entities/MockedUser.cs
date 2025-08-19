namespace Tests;

public class MockedUser
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string AboutMe { get; set; } = String.Empty;

    private MockedUser(string name, string email, string aboutMe)
    {
        Name = name;
        Email = email;
        AboutMe = aboutMe;
    }

    public class UserBuilder
    {
        private string _name = String.Empty;
        private string _email = string.Empty;
        private string _aboutMe = string.Empty;

        public UserBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public UserBuilder SetEmail(string email)
        {
            _email = email;
            return this;
        }

        public UserBuilder SetAboutMe(string aboutMe)
        {
            _aboutMe = aboutMe;
            return this;
        }

        public MockedUser Build()
        {
            return new MockedUser(_name,_email,_aboutMe);
        }
    }
}
