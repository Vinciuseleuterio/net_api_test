using Domain.Interfaces.Entities;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;

namespace Domain.Entities;

public class User : StandardModel, ISoftDelete
{
    public string Name { get; private set; }
    public string Email { get; }
    public string AboutMe { get; private set; }
    public List<Note>? Notes { get; }
    public List<Group>? Groups { get; }
    public List<GroupMembership>? GroupMemberships { get; }

    private User(string name, string email, string aboutMe)
    {
        Name = name;
        Email = email;
        AboutMe = aboutMe;
    }

    public class UserBuilder : IUser
    {
        private string _name = "";
        private string _email = "";
        private string _aboutMe = "";

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
            if (String.IsNullOrWhiteSpace(_name))
                throw new InvalidOperationException("Name can't be null or empty");

            if (String.IsNullOrWhiteSpace(_email))
                throw new InvalidOperationException("Email can't be null or empty");

            return new User(_name, _email, _aboutMe);
        }

        public User Update(User user)
        {
            user.Name = _name;
            user.AboutMe = _aboutMe;

            return user;
        }
    }
}
