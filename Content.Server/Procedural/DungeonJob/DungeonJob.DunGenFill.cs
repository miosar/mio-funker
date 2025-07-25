// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using System.Threading.Tasks;
using Content.Shared.Maps;
using Content.Shared.Procedural;
using Content.Shared.Procedural.DungeonGenerators;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="FillGridDunGen"/>
    /// </summary>
    private async Task<Dungeon> GenerateFillDunGen(FillGridDunGen fill, DungeonData data, HashSet<Vector2i> reservedTiles)
    {
        if (!data.Entities.TryGetValue(DungeonDataKey.Fill, out var fillEnt))
        {
            LogDataError(typeof(FillGridDunGen));
            return Dungeon.Empty;
        }

        var roomTiles = new HashSet<Vector2i>();
        var tiles = _maps.GetAllTilesEnumerator(_gridUid, _grid);

        while (tiles.MoveNext(out var tileRef))
        {
            var tile = tileRef.Value.GridIndices;

            if (reservedTiles.Contains(tile))
                continue;

            if (fill.AllowedTiles != null && !fill.AllowedTiles.Contains(((ContentTileDefinition) _tileDefManager[tileRef.Value.Tile.TypeId]).ID))
                continue;

            if (!_anchorable.TileFree(_grid, tile, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                continue;

            var gridPos = _maps.GridTileToLocal(_gridUid, _grid, tile);
            _entManager.SpawnEntity(fillEnt, gridPos);

            roomTiles.Add(tile);

            await SuspendDungeon();
            if (!ValidateResume())
                break;
        }

        var dungeon = new Dungeon();
        var room = new DungeonRoom(roomTiles, Vector2.Zero, Box2i.Empty, new HashSet<Vector2i>());
        dungeon.AddRoom(room);

        return dungeon;
    }
}
