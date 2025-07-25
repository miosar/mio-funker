// SPDX-FileCopyrightText: 2023 Simon <63975668+Simyon264@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Zombies;

/// <summary>
/// Overrides the applied accent for zombies.
/// </summary>
[RegisterComponent]
public sealed partial class ZombieAccentOverrideComponent : Component
{
    [DataField("accent")]
    public string Accent = "zombie";
}
