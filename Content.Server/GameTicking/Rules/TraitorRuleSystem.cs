// SPDX-FileCopyrightText: 2021 Morbo <exstrominer@gmail.com>
// SPDX-FileCopyrightText: 2021 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 drakewill-CRL <46307022+drakewill-CRL@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 moonheart08 <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Ed <96445749+TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 2023 OctoRocket <88291550+OctoRocket@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 2023 ShadowCommander <10494922+ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Skye <22365940+Skyedra@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Tom Leys <tom@gridspy.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Vordenburg <114301317+Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Vyacheslav Titov <rincew1nd@ya.ru>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 router <messagebus@vk.com>
// SPDX-FileCopyrightText: 2024 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2024 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2024 Cojoke <83733158+Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Errant <35878406+Errant-4@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2024 Rainfey <rainfey0+github@gmail.com>
// SPDX-FileCopyrightText: 2024 SlamBamActionman <83650252+SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 username <113782077+whateverusername0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration.Logs;
using Content.Server.Antag;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Mind;
using Content.Server.Objectives;
using Content.Server.PDA.Ringer;
using Content.Server.Roles;
using Content.Server.Traitor.Uplink;
using Content.Shared.Database;
using Content.Shared.GameTicking.Components;
using Content.Shared.Mind;
using Content.Shared.NPC.Systems;
using Content.Shared.PDA;
using Content.Shared.Roles;
using Content.Shared.Roles.Jobs;
using Content.Shared.Roles.RoleCodeword;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using System.Linq;
using System.Text;

namespace Content.Server.GameTicking.Rules;

/// <summary>
/// :trollface:
/// Hi, tay here.
/// The goob folk also said that this is heavily edited. I'm here to say the same thing as well!
/// Take care now!
/// </summary>
public sealed class TraitorRuleSystem : GameRuleSystem<TraitorRuleComponent>
{
    private static readonly Color TraitorCodewordColor = Color.FromHex("#cc3b3b");

    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly AntagSelectionSystem _antag = default!;
    [Dependency] private readonly SharedJobSystem _jobs = default!;
    [Dependency] private readonly MindSystem _mindSystem = default!;
    [Dependency] private readonly NpcFactionSystem _npcFaction = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedRoleCodewordSystem _roleCodewordSystem = default!;
    [Dependency] private readonly SharedRoleSystem _roleSystem = default!;
    [Dependency] private readonly UplinkSystem _uplink = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TraitorRuleComponent, AfterAntagEntitySelectedEvent>(AfterEntitySelected);
        SubscribeLocalEvent<TraitorRuleComponent, ObjectivesTextPrependEvent>(OnObjectivesTextPrepend);
    }

    protected override void Added(EntityUid uid, TraitorRuleComponent component, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        base.Added(uid, component, gameRule, args);
        SetCodewords(component, args.RuleEntity);
    }

    private void AfterEntitySelected(Entity<TraitorRuleComponent> ent, ref AfterAntagEntitySelectedEvent args)
    {
        MakeTraitor(args.EntityUid, ent);
    }

    private void SetCodewords(TraitorRuleComponent component, EntityUid ruleEntity)
    {
        component.Codewords = GenerateTraitorCodewords(component);
        _adminLogger.Add(LogType.EventStarted, LogImpact.Low, $"Codewords generated for game rule {ToPrettyString(ruleEntity)}: {string.Join(", ", component.Codewords)}");
    }

    public string[] GenerateTraitorCodewords(TraitorRuleComponent component)
    {
        var adjectives = _prototypeManager.Index(component.CodewordAdjectives).Values;
        var verbs = _prototypeManager.Index(component.CodewordVerbs).Values;
        var codewordPool = adjectives.Concat(verbs).ToList();
        var finalCodewordCount = Math.Min(component.CodewordCount, codewordPool.Count);
        string[] codewords = new string[finalCodewordCount];
        for (var i = 0; i < finalCodewordCount; i++)
        {
            codewords[i] = _random.PickAndTake(codewordPool);
        }
        return codewords;
    }

    public bool MakeTraitor(EntityUid traitor, TraitorRuleComponent component, bool giveUplink = true)
    {
        //Grab the mind if it wasn't provided
        if (!_mindSystem.TryGetMind(traitor, out var mindId, out var mind))
            return false;

        var briefing = Loc.GetString("traitor-role-codewords-short", ("codewords", string.Join(", ", component.Codewords)));
        var issuer = _random.Pick(_prototypeManager.Index(component.ObjectiveIssuers).Values);
        component.ObjectiveIssuer = issuer;

        Note[]? code = null;
        if (giveUplink)
        {
            // Calculate the amount of currency on the uplink.
            var startingBalance = component.StartingBalance;
            if (_jobs.MindTryGetJob(mindId, out var prototype))
                startingBalance = Math.Max(startingBalance - prototype.AntagAdvantage, 0);

            // creadth: we need to create uplink for the antag.
            // PDA should be in place already
            var pda = _uplink.FindUplinkTarget(traitor);
            if (pda == null || !_uplink.AddUplink(traitor, startingBalance))
                return false;

            // Give traitors their codewords and uplink code to keep in their character info menu
            code = EnsureComp<RingerUplinkComponent>(pda.Value).Code;

            // If giveUplink is false the uplink code part is omitted
            briefing = string.Format("{0}\n{1}", briefing,
                Loc.GetString("traitor-role-uplink-code-short", ("code", string.Join("-", code).Replace("sharp", "#"))));
        }

        _antag.SendBriefing(traitor, GenerateBriefing(component.Codewords, code, issuer), Color.Crimson, component.GreetSoundNotification);

        component.TraitorMinds.Add(mindId);

        // Assign briefing
        //Since this provides neither an antag/job prototype, nor antag status/roletype,
        //and is intrinsically related to the traitor role
        //it does not need to be a separate Mind Role Entity
        _roleSystem.MindHasRole<TraitorRoleComponent>(mindId, out var traitorRole);
        if (traitorRole is not null)
        {
            AddComp<RoleBriefingComponent>(traitorRole.Value.Owner);
            Comp<RoleBriefingComponent>(traitorRole.Value.Owner).Briefing = GenerateBriefingCharacter(component.Codewords, code, issuer);
        }

        // Send codewords to only the traitor client
        var color = TraitorCodewordColor; // Fall back to a dark red Syndicate color if a prototype is not found

        RoleCodewordComponent codewordComp = EnsureComp<RoleCodewordComponent>(mindId);
        _roleCodewordSystem.SetRoleCodewords(codewordComp, "traitor", component.Codewords.ToList(), color);

        // Change the faction
        _npcFaction.RemoveFaction(traitor, component.NanoTrasenFaction, false);
        _npcFaction.AddFaction(traitor, component.SyndicateFaction);

        return true;
    }

    // TODO: AntagCodewordsComponent
    private void OnObjectivesTextPrepend(EntityUid uid, TraitorRuleComponent comp, ref ObjectivesTextPrependEvent args)
    {
        args.Text += "\n" + Loc.GetString("traitor-round-end-codewords", ("codewords", string.Join(", ", comp.Codewords)));
    }

    // TODO: figure out how to handle this? add priority to briefing event?
    private string GenerateBriefing(string[] codewords, Note[]? uplinkCode, string objectiveIssuer)
    {
        var sb = new StringBuilder();
        sb.AppendLine("\n" + Loc.GetString($"traitor-{objectiveIssuer}-intro"));

        if (uplinkCode != null)
        {
            sb.AppendLine("\n" + Loc.GetString($"traitor-{objectiveIssuer}-uplink"));
            sb.AppendLine(Loc.GetString($"traitor-role-uplink-code", ("code", string.Join("-", uplinkCode).Replace("sharp", "#"))));
        }
        else sb.AppendLine("\n" + Loc.GetString($"traitor-role-nouplink"));

        sb.AppendLine("\n" + Loc.GetString($"traitor-role-codewords", ("codewords", string.Join(", ", codewords))));

        sb.AppendLine("\n" + Loc.GetString($"traitor-role-moreinfo"));

        return sb.ToString();
    }
    private string GenerateBriefingCharacter(string[] codewords, Note[]? uplinkCode, string objectiveIssuer)
    {
        var sb = new StringBuilder();
        sb.AppendLine("\n" + Loc.GetString($"traitor-{objectiveIssuer}-intro"));

        if (uplinkCode != null)
            sb.AppendLine(Loc.GetString($"traitor-role-uplink-code-short", ("code", string.Join("-", uplinkCode).Replace("sharp", "#"))));
        else sb.AppendLine("\n" + Loc.GetString($"traitor-role-nouplink"));

        sb.AppendLine(Loc.GetString($"traitor-role-codewords-short", ("codewords", string.Join(", ", codewords))));

        sb.AppendLine("\n" + Loc.GetString($"traitor-role-allegiances"));
        sb.AppendLine(Loc.GetString($"traitor-{objectiveIssuer}-allies"));

        sb.AppendLine("\n" + Loc.GetString($"traitor-role-notes"));
        sb.AppendLine(Loc.GetString($"traitor-{objectiveIssuer}-goal"));

        return sb.ToString();
    }

    public List<(EntityUid Id, MindComponent Mind)> GetOtherTraitorMindsAliveAndConnected(MindComponent ourMind)
    {
        List<(EntityUid Id, MindComponent Mind)> allTraitors = new();

        var query = EntityQueryEnumerator<TraitorRuleComponent>();
        while (query.MoveNext(out var uid, out var traitor))
        {
            foreach (var role in GetOtherTraitorMindsAliveAndConnected(ourMind, (uid, traitor)))
            {
                if (!allTraitors.Contains(role))
                    allTraitors.Add(role);
            }
        }

        return allTraitors;
    }

    private List<(EntityUid Id, MindComponent Mind)> GetOtherTraitorMindsAliveAndConnected(MindComponent ourMind, Entity<TraitorRuleComponent> rule)
    {
        var traitors = new List<(EntityUid Id, MindComponent Mind)>();
        foreach (var mind in _antag.GetAntagMinds(rule.Owner))
        {
            if (mind.Comp == ourMind)
                continue;

            traitors.Add((mind, mind));
        }

        return traitors;
    }
}
