// SPDX-FileCopyrightText: 2024 brainfood1183 <113240905+brainfood1183@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Atmos.Components;
using Content.Shared.Actions.Events;
using Content.Shared.Atmos.Components;
using Content.Shared.Atmos.EntitySystems;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Map;

namespace Content.Server.Atmos.EntitySystems;

/// <summary>
/// Adds an action ability that will cause all flammable targets in a radius to ignite, also heals the owner
/// of the component when used.
/// </summary>
public sealed class FirestarterSystem : SharedFirestarterSystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly FlammableSystem _flammable = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;

    private readonly HashSet<Entity<FlammableComponent>> _flammables = new();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<FirestarterComponent, FireStarterActionEvent>(OnStartFire);
    }

    /// <summary>
    /// Checks Radius for igniting nearby flammable objects
    /// </summary>
    private void OnStartFire(EntityUid uid, FirestarterComponent component, FireStarterActionEvent args)
    {
        if (_container.IsEntityOrParentInContainer(uid))
            return;

        var xform = Transform(uid);
        var ignitionRadius = component.IgnitionRadius;
        IgniteNearby(uid, xform.Coordinates, args.Severity, ignitionRadius);
        _audio.PlayPvs(component.IgniteSound, uid);

        args.Handled = true;
    }

    /// <summary>
    /// Ignites flammable objects within range.
    /// </summary>
    public void IgniteNearby(EntityUid uid, EntityCoordinates coordinates, float severity, float radius)
    {
        _flammables.Clear();
        _lookup.GetEntitiesInRange(coordinates, radius, _flammables);

        foreach (var flammable in _flammables)
        {
            var ent = flammable.Owner;
            var stackAmount = 2 + (int) (severity / 0.15f);
            _flammable.AdjustFireStacks(ent, stackAmount, flammable);
            _flammable.Ignite(ent, uid, flammable);
        }
    }
}
