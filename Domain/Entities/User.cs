using NotesApp.Domain.Interfaces;

namespace NotesApp.Domain.Entities
{
    public class User : StandardModel, ISoftDelete
    {
        public string Name { get; }
        public string Email { get; }
        public string AboutMe { get; } = String.Empty;
        public List<Note> Notes { get; } = [];
        public List<Group> Groups { get; } = [];
        public List<GroupMembership> GroupMemberships { get; } = [];

        private User(string name, string email, string aboutMe)
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

            public User Build()
            {
                if (string.IsNullOrEmpty(_name))
                    throw new InvalidOperationException("Name must be set before Build()");

                if (string.IsNullOrEmpty(_email))
                    throw new InvalidOperationException("Email must be set before Build()");

                return new User(_name, _email, _aboutMe);
            }
        }
    }
}