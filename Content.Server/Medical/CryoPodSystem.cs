// SPDX-FileCopyrightText: 2022 Francesco <frafonia@gmail.com>
// SPDX-FileCopyrightText: 2023 Chief-Engineer <119664036+Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 Emisse <99158783+Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 faint <46868845+ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Cojoke <83733158+Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Ed <96445749+TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 2024 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 2024 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2024 Rainfey <rainfey0+github@gmail.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2024 Whisper <121047731+QuietlyWhisper@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 nikthechampiongr <32041239+nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 themias <89101928+themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration.Logs;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Components;
using Content.Server.Atmos.Piping.Unary.EntitySystems;
using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.Medical.Components;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.NodeGroups;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Temperature.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.UserInterface;
using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Climbing.Systems;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.DragDrop;
using Content.Shared.Emag.Systems;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Medical.Cryogenics;
using Content.Shared.MedicalScanner;
using Content.Shared.Power;
using Content.Shared.Verbs;
using Robust.Server.GameObjects;
using Robust.Shared.Containers;
using Robust.Shared.Timing;
using SharedToolSystem = Content.Shared.Tools.Systems.SharedToolSystem;

namespace Content.Server.Medical;

public sealed partial class CryoPodSystem : SharedCryoPodSystem
{
    [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;
    [Dependency] private readonly GasCanisterSystem _gasCanisterSystem = default!;
    [Dependency] private readonly ClimbSystem _climbSystem = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlotsSystem = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainerSystem = default!;
    [Dependency] private readonly BloodstreamSystem _bloodstreamSystem = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;
    [Dependency] private readonly SharedToolSystem _toolSystem = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly MetaDataSystem _metaDataSystem = default!;
    [Dependency] private readonly ReactiveSystem _reactiveSystem = default!;
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly NodeContainerSystem _nodeContainer = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CryoPodComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<CryoPodComponent, GetVerbsEvent<AlternativeVerb>>(AddAlternativeVerbs);
        SubscribeLocalEvent<CryoPodComponent, GotEmaggedEvent>(OnEmagged);
        SubscribeLocalEvent<CryoPodComponent, CryoPodDragFinished>(OnDragFinished);
        SubscribeLocalEvent<CryoPodComponent, CryoPodPryFinished>(OnCryoPodPryFinished);

        SubscribeLocalEvent<CryoPodComponent, AtmosDeviceUpdateEvent>(OnCryoPodUpdateAtmosphere);
        SubscribeLocalEvent<CryoPodComponent, DragDropTargetEvent>(HandleDragDropOn);
        SubscribeLocalEvent<CryoPodComponent, InteractUsingEvent>(OnInteractUsing);
        SubscribeLocalEvent<CryoPodComponent, ExaminedEvent>(OnExamined);
        SubscribeLocalEvent<CryoPodComponent, PowerChangedEvent>(OnPowerChanged);
        SubscribeLocalEvent<CryoPodComponent, GasAnalyzerScanEvent>(OnGasAnalyzed);
        SubscribeLocalEvent<CryoPodComponent, ActivatableUIOpenAttemptEvent>(OnActivateUIAttempt);
        SubscribeLocalEvent<CryoPodComponent, AfterActivatableUIOpenEvent>(OnActivateUI);
        SubscribeLocalEvent<CryoPodComponent, EntRemovedFromContainerMessage>(OnEjected);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var curTime = _gameTiming.CurTime;
        var bloodStreamQuery = GetEntityQuery<BloodstreamComponent>();
        var metaDataQuery = GetEntityQuery<MetaDataComponent>();
        var itemSlotsQuery = GetEntityQuery<ItemSlotsComponent>();
        var fitsInDispenserQuery = GetEntityQuery<FitsInDispenserComponent>();
        var solutionContainerManagerQuery = GetEntityQuery<SolutionContainerManagerComponent>();
        var query = EntityQueryEnumerator<ActiveCryoPodComponent, CryoPodComponent>();

        while (query.MoveNext(out var uid, out _, out var cryoPod))
        {
            metaDataQuery.TryGetComponent(uid, out var metaDataComponent);
            if (curTime < cryoPod.NextInjectionTime + _metaDataSystem.GetPauseTime(uid, metaDataComponent))
                continue;
            cryoPod.NextInjectionTime = curTime + TimeSpan.FromSeconds(cryoPod.BeakerTransferTime);

            if (!itemSlotsQuery.TryGetComponent(uid, out var itemSlotsComponent))
            {
                continue;
            }
            var container = _itemSlotsSystem.GetItemOrNull(uid, cryoPod.SolutionContainerName, itemSlotsComponent);
            var patient = cryoPod.BodyContainer.ContainedEntity;
            if (container != null
                && container.Value.Valid
                && patient != null
                && fitsInDispenserQuery.TryGetComponent(container, out var fitsInDispenserComponent)
                && solutionContainerManagerQuery.TryGetComponent(container,
                    out var solutionContainerManagerComponent)
                && _solutionContainerSystem.TryGetFitsInDispenser((container.Value, fitsInDispenserComponent, solutionContainerManagerComponent),
                    out var containerSolution, out _))
            {
                if (!bloodStreamQuery.TryGetComponent(patient, out var bloodstream))
                {
                    continue;
                }

                var solutionToInject = _solutionContainerSystem.SplitSolution(containerSolution.Value, cryoPod.BeakerTransferAmount);
                _bloodstreamSystem.TryAddToChemicals(patient.Value, solutionToInject, bloodstream);
                _reactiveSystem.DoEntityReaction(patient.Value, solutionToInject, ReactionMethod.Injection);
            }
        }
    }

    public override EntityUid? EjectBody(EntityUid uid, CryoPodComponent? cryoPodComponent)
    {
        if (!Resolve(uid, ref cryoPodComponent))
            return null;
        if (cryoPodComponent.BodyContainer.ContainedEntity is not { Valid: true } contained)
            return null;
        base.EjectBody(uid, cryoPodComponent);
        _climbSystem.ForciblySetClimbing(contained, uid);
        return contained;
    }

    #region Interaction

    private void HandleDragDropOn(Entity<CryoPodComponent> entity, ref DragDropTargetEvent args)
    {
        if (entity.Comp.BodyContainer.ContainedEntity != null)
            return;

        var doAfterArgs = new DoAfterArgs(EntityManager, args.User, entity.Comp.EntryDelay, new CryoPodDragFinished(), entity, target: args.Dragged, used: entity)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = false,
        };
        _doAfterSystem.TryStartDoAfter(doAfterArgs);
        args.Handled = true;
    }

    private void OnDragFinished(Entity<CryoPodComponent> entity, ref CryoPodDragFinished args)
    {
        if (args.Cancelled || args.Handled || args.Args.Target == null)
            return;

        if (InsertBody(entity.Owner, args.Args.Target.Value, entity.Comp))
        {
            if (!TryComp(entity.Owner, out CryoPodAirComponent? cryoPodAir))
                _adminLogger.Add(LogType.Action, LogImpact.Medium,
                    $"{ToPrettyString(args.User)} inserted {ToPrettyString(args.Args.Target.Value)} into {ToPrettyString(entity.Owner)}");

            _adminLogger.Add(LogType.Action, LogImpact.Medium,
                $"{ToPrettyString(args.User)} inserted {ToPrettyString(args.Args.Target.Value)} into {ToPrettyString(entity.Owner)} which contains gas: {cryoPodAir!.Air.ToPrettyString():gasMix}");
        }
        args.Handled = true;
    }

    private void OnActivateUIAttempt(Entity<CryoPodComponent> entity, ref ActivatableUIOpenAttemptEvent args)
    {
        if (args.Cancelled)
        {
            return;
        }

        var containedEntity = entity.Comp.BodyContainer.ContainedEntity;
        if (containedEntity == null || containedEntity == args.User || !HasComp<ActiveCryoPodComponent>(entity))
        {
            args.Cancel();
        }
    }

    private void OnActivateUI(Entity<CryoPodComponent> entity, ref AfterActivatableUIOpenEvent args)
    {
        if (!entity.Comp.BodyContainer.ContainedEntity.HasValue)
            return;

        TryComp<TemperatureComponent>(entity.Comp.BodyContainer.ContainedEntity, out var temp);
        TryComp<BloodstreamComponent>(entity.Comp.BodyContainer.ContainedEntity, out var bloodstream);

        if (TryComp<HealthAnalyzerComponent>(entity, out var healthAnalyzer))
        {
            healthAnalyzer.ScannedEntity = entity.Comp.BodyContainer.ContainedEntity;
        }

        // TODO: This should be a state my dude
        _uiSystem.ServerSendUiMessage(
            entity.Owner,
            HealthAnalyzerUiKey.Key,
            new HealthAnalyzerScannedUserMessage(GetNetEntity(entity.Comp.BodyContainer.ContainedEntity),
                temp?.CurrentTemperature ?? 0,
                (bloodstream != null && _solutionContainerSystem.ResolveSolution(entity.Comp.BodyContainer.ContainedEntity.Value,
                    bloodstream.BloodSolutionName, ref bloodstream.BloodSolution, out var bloodSolution))
                    ? bloodSolution.FillFraction
                    : 0,
                null,
                null,
                null,
                null // Shitmed Change
            ));
    }

    private void OnInteractUsing(Entity<CryoPodComponent> entity, ref InteractUsingEvent args)
    {
        if (args.Handled || !entity.Comp.Locked || entity.Comp.BodyContainer.ContainedEntity == null)
            return;

        args.Handled = _toolSystem.UseTool(args.Used, args.User, entity.Owner, entity.Comp.PryDelay, "Prying", new CryoPodPryFinished());
    }

    private void OnExamined(Entity<CryoPodComponent> entity, ref ExaminedEvent args)
    {
        var container = _itemSlotsSystem.GetItemOrNull(entity.Owner, entity.Comp.SolutionContainerName);
        if (args.IsInDetailsRange && container != null && _solutionContainerSystem.TryGetFitsInDispenser(container.Value, out _, out var containerSolution))
        {
            using (args.PushGroup(nameof(CryoPodComponent)))
            {
                args.PushMarkup(Loc.GetString("cryo-pod-examine", ("beaker", Name(container.Value))));
                if (containerSolution.Volume == 0)
                {
                    args.PushMarkup(Loc.GetString("cryo-pod-empty-beaker"));
                }
            }
        }
    }

    private void OnPowerChanged(Entity<CryoPodComponent> entity, ref PowerChangedEvent args)
    {
        // Needed to avoid adding/removing components on a deleted entity
        if (Terminating(entity))
        {
            return;
        }

        if (args.Powered)
        {
            EnsureComp<ActiveCryoPodComponent>(entity);
        }
        else
        {
            RemComp<ActiveCryoPodComponent>(entity);
            _uiSystem.CloseUi(entity.Owner, HealthAnalyzerUiKey.Key);
        }
        UpdateAppearance(entity.Owner, entity.Comp);
    }

    #endregion

    #region Atmos handler

    private void OnCryoPodUpdateAtmosphere(Entity<CryoPodComponent> entity, ref AtmosDeviceUpdateEvent args)
    {
        if (!_nodeContainer.TryGetNode(entity.Owner, entity.Comp.PortName, out PortablePipeNode? portNode))
            return;

        if (!TryComp(entity, out CryoPodAirComponent? cryoPodAir))
            return;

        _atmosphereSystem.React(cryoPodAir.Air, portNode);

        if (portNode.NodeGroup is PipeNet { NodeCount: > 1 } net)
        {
            _gasCanisterSystem.MixContainerWithPipeNet(cryoPodAir.Air, net.Air);
        }
    }

    private void OnGasAnalyzed(Entity<CryoPodComponent> entity, ref GasAnalyzerScanEvent args)
    {
        if (!TryComp(entity, out CryoPodAirComponent? cryoPodAir))
            return;

        args.GasMixtures ??= new List<(string, GasMixture?)>();
        args.GasMixtures.Add((Name(entity.Owner), cryoPodAir.Air));
        // If it's connected to a port, include the port side
        // multiply by volume fraction to make sure to send only the gas inside the analyzed pipe element, not the whole pipe system
        if (_nodeContainer.TryGetNode(entity.Owner, entity.Comp.PortName, out PipeNode? port) && port.Air.Volume != 0f)
        {
            var portAirLocal = port.Air.Clone();
            portAirLocal.Multiply(port.Volume / port.Air.Volume);
            portAirLocal.Volume = port.Volume;
            args.GasMixtures.Add((entity.Comp.PortName, portAirLocal));
        }
    }

    private void OnEjected(Entity<CryoPodComponent> cryoPod, ref EntRemovedFromContainerMessage args)
    {
        if (TryComp<HealthAnalyzerComponent>(cryoPod.Owner, out var healthAnalyzer))
        {
            healthAnalyzer.ScannedEntity = null;
        }

        // if body is ejected - no need to display health-analyzer
        _uiSystem.CloseUi(cryoPod.Owner, HealthAnalyzerUiKey.Key);
    }

    #endregion
}
