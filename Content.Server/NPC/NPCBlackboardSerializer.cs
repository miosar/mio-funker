// SPDX-FileCopyrightText: 2022 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Reflection;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;
using Robust.Shared.Utility;

namespace Content.Server.NPC;

public sealed class NPCBlackboardSerializer : ITypeReader<NPCBlackboard, MappingDataNode>, ITypeCopier<NPCBlackboard>
{
    public ValidationNode Validate(ISerializationManager serializationManager, MappingDataNode node,
        IDependencyCollection dependencies, ISerializationContext? context = null)
    {
        var validated = new List<ValidationNode>();

        if (node.Count <= 0)
            return new ValidatedSequenceNode(validated);

        var reflection = dependencies.Resolve<IReflectionManager>();

        foreach (var data in node)
        {
            var key = data.Key;

            if (data.Value.Tag == null)
            {
                validated.Add(new ErrorNode(node.GetKeyNode(key), $"Unable to validate {key}'s type"));
                continue;
            }

            var typeString = data.Value.Tag[6..];

            if (!reflection.TryLooseGetType(typeString, out var type))
            {
                validated.Add(new ErrorNode(node.GetKeyNode(key), $"Unable to find type for {typeString}"));
                continue;
            }

            var validatedNode = serializationManager.ValidateNode(type, data.Value, context);
            validated.Add(validatedNode);
        }

        return new ValidatedSequenceNode(validated);
    }

    public NPCBlackboard Read(ISerializationManager serializationManager, MappingDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx, ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<NPCBlackboard>? instanceProvider = null)
    {
        var value = instanceProvider != null ? instanceProvider() : new NPCBlackboard();

        if (node.Count <= 0)
            return value;

        var reflection = dependencies.Resolve<IReflectionManager>();

        foreach (var data in node)
        {
            var key = data.Key;

            if (data.Value.Tag == null)
                throw new NullReferenceException($"Found null tag for {key}");

            var typeString = data.Value.Tag[6..];

            if (!reflection.TryLooseGetType(typeString, out var type))
                throw new NullReferenceException($"Found null type for {key}");

            var bbData = serializationManager.Read(type, data.Value, hookCtx, context);

            if (bbData == null)
                throw new NullReferenceException($"Found null data for {key}, expected {type}");

            value.SetValue(key, bbData);
        }

        return value;
    }

    public void CopyTo(
        ISerializationManager serializationManager,
        NPCBlackboard source,
        ref NPCBlackboard target,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null)
    {
        target.Clear();
        using var enumerator = source.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            target.SetValue(current.Key, current.Value);
        }
    }
}
