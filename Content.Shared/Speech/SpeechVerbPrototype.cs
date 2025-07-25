// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.Speech;

/// <summary>
///     Handles replacing speech verbs and other conditional chat modifications like bolding or font type depending
///     on punctuation or by directly overriding the prototype.
/// </summary>
[Prototype]
public sealed partial class SpeechVerbPrototype : IPrototype
{
    [IdDataField] public string ID { get; private set; } = default!;

    /// <summary>
    ///     Loc strings to be passed to the chat wrapper. 'says', 'states', etc.
    ///     Picks one at random if there are multiple.
    /// </summary>
    [DataField("speechVerbStrings", required: true)]
    public List<string> SpeechVerbStrings = default!;

    /// <summary>
    ///     Should use of this speech verb bold the corresponding message?
    /// </summary>
    [DataField("bold")]
    public bool Bold = false;

    /// <summary>
    ///     What font size should be used for the message contents?
    /// </summary>
    [DataField("fontSize")]
    public int FontSize = 12;

    /// <summary>
    ///     What font prototype ID should be used for the message contents?
    /// </summary>
    /// font proto is client only so cant lint this lol sorry
    [DataField("fontId")]
    public string FontId = "Default";

    /// <summary>
    ///     If multiple applicable speech verb protos are found (i.e. through speech suffixes) this will determine
    ///     which one is picked. Higher = more priority.
    /// </summary>
    [DataField("priority")]
    public int Priority = 0;

    /// <summary>
    /// Name shown in the voicemask UI for this verb.
    /// </summary>
    [DataField(required: true)]
    public LocId Name = string.Empty;
}
