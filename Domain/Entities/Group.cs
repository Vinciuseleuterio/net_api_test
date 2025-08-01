using NotesApp.Domain.Interfaces;


namespace NotesApp.Domain.Entities
{
    public class Group : StandardModel, ISoftDelete
    {
        public string Name { get; }
        public string Description { get; } = String.Empty;
        public long CreatorId { get; }
        public User Creator { get; } = null!;
        public List<Note> Notes { get; } = [];
        public List<GroupMembership> GroupMemberships { get; } = [];

        private Group(string name, string description, long creatorId)
        {
            Name = name;
            Description = description;
            CreatorId = creatorId;
        }

        public class GroupBuilder
        {
            private string _name = String.Empty;
            private string _description = String.Empty;
            private long _creatorId = 0;

            public GroupBuilder SetName(string name)
            {
                _name = name;
                return this;
            }

            public GroupBuilder SetDescription(string description)
            {
                _description = description;
                return this;
            }

            public GroupBuilder SetCreatorId(long creatorId)
            {
                _creatorId = creatorId;
                return this;
            }

            public Group Build()
            {
                if (string.IsNullOrEmpty(_name))
                    throw new InvalidOperationException("Name must be set before Build()");

                if (_creatorId <= 0)
                    throw new InvalidOperationException("Email must be set before Build()");

                return new Group(_name, _description, _creatorId);
            }
        }
    }
}
