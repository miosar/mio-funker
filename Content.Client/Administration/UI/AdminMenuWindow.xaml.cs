// SPDX-FileCopyrightText: 2021 Galactic Chimp <63882831+GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Leo <lzimann@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Paul <ritter.paul1+git@googlemail.com>
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 2022 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2024 Chief-Engineer <119664036+Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Hannah Giovanna Dawson <karakkaraz@gmail.com>
// SPDX-FileCopyrightText: 2024 Repo <47093363+Titian3@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.Administration.UI;

[GenerateTypedNameReferences]
public sealed partial class AdminMenuWindow : DefaultWindow
{
    public event Action? OnDisposed;

    public AdminMenuWindow()
    {
        MinSize = new Vector2(650, 250);
        Title = Loc.GetString("admin-menu-title");
        RobustXamlLoader.Load(this);
        MasterTabContainer.SetTabTitle((int) TabIndex.Admin, Loc.GetString("admin-menu-admin-tab"));
        MasterTabContainer.SetTabTitle((int) TabIndex.Adminbus, Loc.GetString("admin-menu-adminbus-tab"));
        MasterTabContainer.SetTabTitle((int) TabIndex.Atmos, Loc.GetString("admin-menu-atmos-tab"));
        MasterTabContainer.SetTabTitle((int) TabIndex.Round, Loc.GetString("admin-menu-round-tab"));
        MasterTabContainer.SetTabTitle((int) TabIndex.Server, Loc.GetString("admin-menu-server-tab"));
        MasterTabContainer.SetTabTitle((int) TabIndex.PanicBunker, Loc.GetString("admin-menu-panic-bunker-tab"));
        MasterTabContainer.SetTabTitle((int) TabIndex.Players, Loc.GetString("admin-menu-players-tab"));
        MasterTabContainer.SetTabTitle((int) TabIndex.Objects, Loc.GetString("admin-menu-objects-tab"));
        MasterTabContainer.OnTabChanged += OnTabChanged;
    }

    private void OnTabChanged(int tabIndex)
    {
        var tabEnum = (TabIndex)tabIndex;
        if (tabEnum == TabIndex.Objects)
            ObjectsTabControl.RefreshObjectList();
    }

    protected override void Dispose(bool disposing)
    {
        OnDisposed?.Invoke();
        base.Dispose(disposing);
        OnDisposed = null;
    }

    private enum TabIndex
    {
        Admin = 0,
        Adminbus,
        Atmos,
        Round,
        Server,
        PanicBunker,
        Players,
        Objects,
    }
}
