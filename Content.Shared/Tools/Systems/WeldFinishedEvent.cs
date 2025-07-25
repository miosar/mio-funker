// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Tools.Systems;

/// <summary>
///     Raised after welding do_after has finished. It doesn't guarantee success,
///     use <see cref="WeldableChangedEvent"/> to get updated status.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class WeldFinishedEvent : SimpleDoAfterEvent
{
}
