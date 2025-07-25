// SPDX-FileCopyrightText: 2022 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Chief-Engineer <119664036+Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Riggle <27156122+RigglePrime@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Database;
using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Administration.Notes;

[Serializable, NetSerializable]
public sealed class AdminNotesEuiState : EuiStateBase
{
    public AdminNotesEuiState(string notedPlayerName, Dictionary<(int, NoteType), SharedAdminNote> notes, bool canCreate, bool canDelete, bool canEdit)
    {
        NotedPlayerName = notedPlayerName;
        Notes = notes;
        CanCreate = canCreate;
        CanDelete = canDelete;
        CanEdit = canEdit;
    }

    public string NotedPlayerName { get; }
    public Dictionary<(int noteId, NoteType noteType), SharedAdminNote> Notes { get; }
    public bool CanCreate { get; }
    public bool CanDelete { get; }
    public bool CanEdit { get; }
}

public static class AdminNoteEuiMsg
{
    [Serializable, NetSerializable]
    public sealed class CreateNoteRequest : EuiMessageBase
    {
        public CreateNoteRequest(NoteType type, string message, NoteSeverity? severity, bool secret, DateTime? expiryTime)
        {
            NoteType = type;
            Message = message;
            NoteSeverity = severity;
            Secret = secret;
            ExpiryTime = expiryTime;
        }

        public NoteType NoteType { get; set; }
        public string Message { get; set; }
        public NoteSeverity? NoteSeverity { get; set; }
        public bool Secret { get; set; }
        public DateTime? ExpiryTime { get; set; }
    }

    [Serializable, NetSerializable]
    public sealed class DeleteNoteRequest : EuiMessageBase
    {
        public DeleteNoteRequest(int id, NoteType type)
        {
            Id = id;
            Type = type;
        }

        public int Id { get; set; }
        public NoteType Type { get; set; }
    }

    [Serializable, NetSerializable]
    public sealed class EditNoteRequest : EuiMessageBase
    {
        public EditNoteRequest(int id, NoteType type, string message, NoteSeverity? severity, bool secret, DateTime? expiryTime)
        {
            Id = id;
            Type = type;
            Message = message;
            NoteSeverity = severity;
            Secret = secret;
            ExpiryTime = expiryTime;
        }

        public int Id { get; set; }
        public NoteType Type { get; set; }
        public string Message { get; set; }
        public NoteSeverity? NoteSeverity { get; set; }
        public bool Secret { get; set; }
        public DateTime? ExpiryTime { get; set; }
    }
}
