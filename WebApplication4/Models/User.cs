using NotesApp.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApp.Models
{
    public class User : StandardModel, ISoftDelete
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? AboutMe { get; set; }
        public List<Note> Notes { get; private set; } = [];
        public List<Group> Groups { get; private set; } = [];
        public List<GroupMembership> GroupMemberships { get; private set; } = [];
    }
}