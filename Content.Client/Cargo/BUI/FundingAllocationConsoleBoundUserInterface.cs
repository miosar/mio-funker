// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Cargo.UI;
using Content.Shared.Cargo.Components;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client.Cargo.BUI;

[UsedImplicitly]
public sealed class FundingAllocationConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [ViewVariables]
    private FundingAllocationMenu? _menu;

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<FundingAllocationMenu>();

        _menu.OnSavePressed += d =>
        {
            SendMessage(new SetFundingAllocationBuiMessage(d));
        };
    }

    protected override void UpdateState(BoundUserInterfaceState message)
    {
        base.UpdateState(message);

        if (message is not FundingAllocationConsoleBuiState state)
            return;

        _menu?.Update(state);
    }
}
