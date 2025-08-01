namespace NotesApp.Domain.Entities
{
    public class Note : StandardModel
    {
        public string Title { get; }
        public string Content { get; } = String.Empty;
        public long CreatorId { get; }
        public User User { get; private set; } = null!;
        public long? GroupId { get; }
        public Group Group { get; private set; } = null!;

        private Note(string title, string content, long creatorId, long? groupId)
        {
            Title = title;
            Content = content;
            CreatorId = creatorId;
            GroupId = groupId;
        }

        public class NoteBuilder
        {
            private string _title = String.Empty;
            private string _content = String.Empty;
            private long _creatorId = 0;
            private long _groupId = 0;

            public NoteBuilder SetTitle(string title)
            {
                _title = title;
                return this;
            }

            public NoteBuilder SetContent(string content)
            {
                _content = content;
                return this;
            }

            public NoteBuilder SetCreatorId(long creatorId)
            {
                if (creatorId <= 0)
                {
                    throw new InvalidOperationException("CreatorId need to be greater than zero");
                }

                _creatorId = creatorId;
                return this;
            }

            public NoteBuilder SetGroupId(long groupId)
            {
                if (groupId <= 0)
                {
                    throw new InvalidOperationException("GroupId need to be greater than zero");
                }

                _groupId = groupId;
                return this;
            }

            public Note Build()
            {
                if (string.IsNullOrEmpty(_title))
                    throw new InvalidOperationException("Title must be set before build");

                if (_creatorId <= 0)
                    throw new InvalidOperationException("CreatorId must be set before build");

                return new Note(_title, _content, _creatorId, _groupId);
            }
        }
    }
}