using NotesApp.Domain.Interfaces;

namespace NotesApp.Domain.Entities
{
    public class User : StandardModel, ISoftDelete
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string AboutMe { get; set; } = String.Empty;
        public List<Note> Notes { get; } = [];
        public List<Group> Groups { get; } = [];
        public List<GroupMembership> GroupMemberships { get; } = [];

        public User(string name, string email, string aboutMe)
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

            public virtual UserBuilder SetName(string name)
            {
                _name = name;
                return this;
            }

            public virtual UserBuilder SetEmail(string email)
            {
                _email = email;
                return this;
            }

            public virtual UserBuilder SetAboutMe(string aboutMe)
            {
                _aboutMe = aboutMe;
                return this;
            }

            public virtual User Build()
            {
                return new User(_name, _email, _aboutMe);
            }

            public virtual User Update(User user)
            {
                user.Name = _name;
                user.AboutMe = _aboutMe;

                return user;
            }
        }
    }
}
