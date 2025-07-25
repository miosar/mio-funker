// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Disposal.Tube.Systems;
using Content.Shared.DeviceLinking;
using Robust.Shared.Prototypes;

namespace Content.Server.Disposal.Tube.Components;

/// <summary>
/// Requires <see cref="DisposalJunctionComponent"/> to function.
/// </summary>
[RegisterComponent, Access(typeof(DisposalSignalRouterSystem))]
public sealed partial class DisposalSignalRouterComponent : Component
{
    /// <summary>
    /// Whether to route items to the side or not.
    /// </summary>
    [DataField]
    public bool Routing;

    /// <summary>
    /// Port that sets the router to send items to the side.
    /// </summary>
    [DataField]
    public ProtoId<SinkPortPrototype> OnPort = "On";

    /// <summary>
    /// Port that sets the router to send items ahead.
    /// </summary>
    [DataField]
    public ProtoId<SinkPortPrototype> OffPort = "Off";

    /// <summary>
    /// Port that toggles the router between sending items to the side and ahead.
    /// </summary>
    [DataField]
    public ProtoId<SinkPortPrototype> TogglePort = "Toggle";
}
