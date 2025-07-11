﻿using NotesApp.Interfaces;
using NotesApp.Models;

namespace WebApplication4.Models
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