// SPDX-FileCopyrightText: 2023 Ed <96445749+TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Objectives.Components;
using Content.Shared.Humanoid;
using Content.Shared.Objectives.Components;

namespace Content.Server.Objectives.Systems;

/// <summary>
/// Handles species requirement for objectives that require a certain species.
/// </summary>
public sealed class SpeciesRequirementSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpeciesRequirementComponent, RequirementCheckEvent>(OnCheck);
    }

    private void OnCheck(Entity<SpeciesRequirementComponent> requirement, ref RequirementCheckEvent args)
    {
        if (args.Cancelled)
            return;

        if (!TryComp<HumanoidAppearanceComponent>(args.Mind.OwnedEntity, out var appearance)) {
            args.Cancelled = true;
            return;
        }
        if (!requirement.Comp.AllowedSpecies.Contains(appearance.Species))
        {
            args.Cancelled = true;
            return;
        }
    }
}
