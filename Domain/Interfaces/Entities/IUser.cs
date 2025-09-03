using Domain.Entities;
using static Domain.Entities.User;

namespace Domain.Interfaces.Entities;

public interface IUser
{
    public UserBuilder SetName(string name);
    public UserBuilder SetEmail(string email);
    public UserBuilder SetAboutMe(string aboutMe);
    public User Build();
    public User Update(User user);
}
