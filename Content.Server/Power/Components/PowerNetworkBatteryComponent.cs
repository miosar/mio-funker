// SPDX-FileCopyrightText: 2021 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Power.Pow3r;
using Content.Shared.Guidebook;

namespace Content.Server.Power.Components
{
    /// <summary>
    ///     Glue component that manages the pow3r network node for batteries that are connected to the power network.
    /// </summary>
    /// <remarks>
    ///     This needs components like <see cref="BatteryChargerComponent"/> to work correctly,
    ///     and battery storage should be handed off to components like <see cref="BatteryComponent"/>.
    /// </remarks>
    [RegisterComponent]
    public sealed partial class PowerNetworkBatteryComponent : Component
    {
        [ViewVariables] public float LastSupply = 0f;

        [DataField("maxChargeRate")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float MaxChargeRate
        {
            get => NetworkBattery.MaxChargeRate;
            set => NetworkBattery.MaxChargeRate = value;
        }

        [DataField("maxSupply")]
        [ViewVariables(VVAccess.ReadWrite)]
        [GuidebookData]
        public float MaxSupply
        {
            get => NetworkBattery.MaxSupply;
            set => NetworkBattery.MaxSupply = value;
        }

        [DataField("supplyRampTolerance")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float SupplyRampTolerance
        {
            get => NetworkBattery.SupplyRampTolerance;
            set => NetworkBattery.SupplyRampTolerance = value;
        }

        [DataField("supplyRampRate")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float SupplyRampRate
        {
            get => NetworkBattery.SupplyRampRate;
            set => NetworkBattery.SupplyRampRate = value;
        }

        [DataField("supplyRampPosition")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float SupplyRampPosition
        {
            get => NetworkBattery.SupplyRampPosition;
            set => NetworkBattery.SupplyRampPosition = value;
        }

        [DataField("currentSupply")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float CurrentSupply
        {
            get => NetworkBattery.CurrentSupply;
            set => NetworkBattery.CurrentSupply = value;
        }

        [DataField("currentReceiving")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float CurrentReceiving
        {
            get => NetworkBattery.CurrentReceiving;
            set => NetworkBattery.CurrentReceiving = value;
        }

        [DataField("loadingNetworkDemand")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float LoadingNetworkDemand
        {
            get => NetworkBattery.LoadingNetworkDemand;
            set => NetworkBattery.LoadingNetworkDemand = value;
        }

        [DataField("enabled")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool Enabled
        {
            get => NetworkBattery.Enabled;
            set => NetworkBattery.Enabled = value;
        }

        [DataField("canCharge")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool CanCharge
        {
            get => NetworkBattery.CanCharge;
            set => NetworkBattery.CanCharge = value;
        }

        [DataField("canDischarge")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool CanDischarge
        {
            get => NetworkBattery.CanDischarge;
            set => NetworkBattery.CanDischarge = value;
        }

        [DataField("efficiency")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float Efficiency
        {
            get => NetworkBattery.Efficiency;
            set => NetworkBattery.Efficiency = value;
        }

        [ViewVariables]
        public PowerState.Battery NetworkBattery { get; } = new();
    }
}
