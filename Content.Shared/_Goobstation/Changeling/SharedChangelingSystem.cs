// SPDX-FileCopyrightText: 2025 TheSecondLord <88201625+TheSecondLord@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._EE.Overlays.Switchable;
using Content.Shared.Eye.Blinding.Components;

namespace Content.Shared.Changeling;

public abstract class SharedChangelingSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ChangelingComponent, SwitchableOverlayToggledEvent>(OnVisionToggle);
    }

    private void OnVisionToggle(Entity<ChangelingComponent> ent, ref SwitchableOverlayToggledEvent args)
    {
        if (args.User != ent.Owner)
            return;

        if (TryComp(ent, out EyeProtectionComponent? eyeProtection))
            eyeProtection.ProtectionTime = args.Activated ? TimeSpan.Zero : TimeSpan.FromSeconds(10);

        UpdateFlashImmunity(ent, !args.Activated);
    }

    protected virtual void UpdateFlashImmunity(EntityUid uid, bool active) { }
}
