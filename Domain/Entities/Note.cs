using NotesApp.Domain.Entities;

namespace Domain.Entities;

public class Note : StandardModel
{
    public string Title { get; private set; }
    public string? Content { get; private set; }
    public long CreatorId { get; }
    public long? GroupId { get; }
    public User? User { get; }
    public Group? Group { get; }

    private Note(string title, string? content, long creatorId, long? groupId)
    {
        Title = title;
        Content = content;
        CreatorId = creatorId;
        GroupId = groupId;
    }

    public class NoteBuilder
    {
        private string _title = "";
        private string _content = "";
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
            _creatorId = creatorId;
            return this;
        }

        public NoteBuilder SetGroupId(long groupId)
        {
            if (groupId <= 0)
                throw new InvalidOperationException("GroupId can't be less than zero or equals to zero");

            _groupId = groupId;
            return this;
        }

        public Note Build()
        {
            if (_creatorId <= 0)
                throw new InvalidOperationException("CreatorId can't be less than zero or equals to zero");

            return new Note(_title, _content, _creatorId, _groupId);
        }

        public Note Update(Note note)
        {
            note.Title = _title;
            note.Content = _content;

            return note;
        }
    }
}
