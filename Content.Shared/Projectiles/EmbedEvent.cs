// SPDX-FileCopyrightText: 2024 Dakamakat <52600490+dakamakat@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Projectiles;

/// <summary>
/// Raised directed on an entity when it embeds in another entity.
/// </summary>
[ByRefEvent]
public readonly record struct EmbedEvent(EntityUid? Shooter, EntityUid Embedded)
{
    public readonly EntityUid? Shooter = Shooter;

    /// <summary>
    /// Entity that is embedded in.
    /// </summary>
    public readonly EntityUid Embedded = Embedded;
}
