﻿using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using WOLF3DGame.Model;

namespace WOLF3DGame
{
    public class Level : Spatial
    {
        public GameMap Map { get; private set; }
        public WorldEnvironment WorldEnvironment { get; private set; }
        public MeshInstance Floor { get; private set; }
        public MeshInstance Ceiling { get; private set; }
        public StaticBody StaticBody { get; private set; }
        public CollisionShape[][] CollisionShapes { get; private set; }

        public bool CanWalk(Vector2 there) => CanWalk(Assets.IntCoordinate(there.x), Assets.IntCoordinate(there.y));

        public bool CanWalk(int x, int z) =>
            !(x < 0 || z < 0 || x >= Map.Width || z >= Map.Depth) &&
            (CollisionShapes[x][z]?.Disabled ?? true);

        public Level(GameMap map)
        {
            Map = map;
            AddChild(WorldEnvironment = new WorldEnvironment()
            {
                Environment = new Godot.Environment()
                {
                    BackgroundColor = Game.Assets.Palette[Map.Border],
                    BackgroundMode = Godot.Environment.BGMode.Color,
                },
            });

            AddChild(Floor = new MeshInstance()
            {
                Mesh = new QuadMesh()
                {
                    Size = new Vector2(Map.Width * Assets.WallWidth, Map.Depth * Assets.WallWidth),
                },
                MaterialOverride = new SpatialMaterial()
                {
                    AlbedoColor = Game.Assets.Palette[Map.Floor],
                    FlagsUnshaded = true,
                    FlagsDoNotReceiveShadows = true,
                    FlagsDisableAmbientLight = true,
                    FlagsTransparent = false,
                    ParamsCullMode = SpatialMaterial.CullMode.Disabled,
                    ParamsSpecularMode = SpatialMaterial.SpecularMode.Disabled,
                },
                Transform = new Transform(
                    Basis.Identity.Rotated(Vector3.Right, Mathf.Pi / 2f),
                    new Vector3(
                        Map.Width * Assets.HalfWallWidth,
                        0f,
                        Map.Depth * Assets.HalfWallWidth
                    )
                ),
            });

            AddChild(Ceiling = new MeshInstance()
            {
                Mesh = new QuadMesh()
                {
                    Size = new Vector2(Map.Width * Assets.WallWidth, Map.Depth * Assets.WallWidth),
                },
                MaterialOverride = new SpatialMaterial()
                {
                    AlbedoColor = Game.Assets.Palette[Map.Ceiling],
                    FlagsUnshaded = true,
                    FlagsDoNotReceiveShadows = true,
                    FlagsDisableAmbientLight = true,
                    FlagsTransparent = false,
                    ParamsCullMode = SpatialMaterial.CullMode.Disabled,
                    ParamsSpecularMode = SpatialMaterial.SpecularMode.Disabled,
                },
                Transform = new Transform(
                    Basis.Identity.Rotated(Vector3.Right, Mathf.Pi / 2f),
                    new Vector3(
                        Map.Width * Assets.HalfWallWidth,
                        (float)Assets.WallHeight,
                        Map.Depth * Assets.HalfWallWidth
                    )
                ),
            });

            MapWalls mapWalls = new MapWalls(Map);
            foreach (Spatial sprite in mapWalls.Walls)
                AddChild(sprite);

            Billboard[] billboards = Billboard.MakeBillboards(Map);
            foreach (Billboard billboard in billboards)
                AddChild(billboard);

            StaticBody = new StaticBody();
            CollisionShapes = new CollisionShape[Map.Width][];
            for (ushort x = 0; x < Map.Width; x++)
            {
                CollisionShapes[x] = new CollisionShape[Map.Depth];
                for (ushort z = 0; z < Map.Depth; z++)
                    if (!IsWall(x, z) || IsByFloor(x, z))
                        StaticBody.AddChild(CollisionShapes[x][z] = new CollisionShape()
                        {
                            Name = "CollisionShape at " + x + ", " + z,
                            Shape = Assets.BoxShape,
                            Disabled = !(IsWall(x, z) || !IsNavigable(x, z)),
                            Transform = new Transform(Basis.Identity, new Vector3(x * Assets.WallWidth + Assets.HalfWallWidth, (float)Assets.HalfWallHeight, z * Assets.WallWidth + Assets.HalfWallWidth)),
                        });
            }
        }

        public bool IsWall(ushort x, ushort z) => IsWall(Map.GetMapData(x, z));

        public static bool IsWall(uint cell) => XWall(cell).Any();

        public bool IsNavigable(ushort x, ushort z) => IsNavigable(Map.GetObjectData(x, z));

        public static bool IsNavigable(uint cell)
        {
            XElement mapObject = (from e in Game.Assets?.XML?.Element("VSwap")?.Element("Objects").Elements("Billboard")
                                  where (uint)e.Attribute("Number") == cell
                                  select e).FirstOrDefault();
            return mapObject == null || Assets.IsTrue(mapObject, "Walk");
        }

        /// <returns>if the specified map coordinate is adjacent to a floor</returns>
        public bool IsByFloor(ushort x, ushort z)
        {
            ushort startX = x < 1 ? x : x > Map.Width - 1 ? (ushort)(Map.Width - 1) : (ushort)(x - 1),
                startZ = z < 1 ? z : z > Map.Depth - 1 ? (ushort)(Map.Depth - 1) : (ushort)(z - 1),
                endX = x >= Map.Width - 1 ? (ushort)(Map.Width - 1) : (ushort)(x + 1),
                endZ = z >= Map.Depth - 1 ? (ushort)(Map.Depth - 1) : (ushort)(z + 1);
            for (ushort dx = startX; dx <= endX; dx++)
                for (ushort dz = startZ; dz <= endZ; dz++)
                    if ((dx != x || dz != z) && !IsWall(dx, dz))
                        return true;
            return false;
        }

        public List<ushort> SquaresOccupied(Vector3 vector3) => SquaresOccupied(Assets.Vector2(vector3));

        public List<ushort> SquaresOccupied(Vector2 vector2)
        {
            List<ushort> list = new List<ushort>();
            void add(Vector2 here)
            {
                int x = Assets.IntCoordinate(here.x), z = Assets.IntCoordinate(here.y);
                if (x >= 0 && z >= 0 && x < Map.Depth && z < Map.Width)
                {
                    ushort square = Map.GetIndex((uint)x, (uint)z);
                    if (!list.Contains(square))
                        list.Add(square);
                }
            }
            add(vector2);
            foreach (Direction8 direction in Direction8.Diagonals)
                add(vector2 + direction.Vector2 * Assets.HeadDiagonal);
            return list;
        }

        public static uint WallTexture(uint cell) =>
            (uint)XWall(cell).FirstOrDefault()?.Attribute("Page");

        /// <summary>
        /// Never underestimate the power of the Dark Side
        /// </summary>
        public static uint DarkSide(uint cell) =>
            (uint)XWall(cell).FirstOrDefault()?.Attribute("DarkSide");

        public static IEnumerable<XElement> XWall(uint cell) =>
            from e in Game.Assets?.XML?.Element("VSwap")?.Element("Walls")?.Elements("Wall") ?? Enumerable.Empty<XElement>()
            where (uint)e.Attribute("Number") == cell
            select e;

        public static IEnumerable<XElement> XDoor(uint cell) =>
            from e in Game.Assets?.XML?.Element("VSwap")?.Element("Walls")?.Elements("Door") ?? Enumerable.Empty<XElement>()
            where (uint)e.Attribute("Number") == cell
            select e;

        public static uint DoorTexture(uint cell) =>
            (uint)XDoor(cell).FirstOrDefault()?.Attribute("Page");

        public static bool IsDoor(uint cell) =>
            XDoor(cell).Any();

        public Transform StartTransform =>
            Start(out ushort index, out Direction8 direction) ?
            new Transform(direction.Basis, new Vector3(Assets.CenterSquare(Map.X(index)), 0f, Assets.CenterSquare(Map.Z(index))))
            : throw new InvalidDataException("Could not find start of level!");

        public bool Start(out ushort index, out Direction8 direction)
        {
            foreach (XElement start in Game.Assets?.XML?.Element("VSwap")?.Element("Objects")?.Elements("Start") ?? Enumerable.Empty<XElement>())
            {
                if (!ushort.TryParse(start.Attribute("Number")?.Value, out ushort find))
                    continue;
                int found = Array.FindIndex(Map.ObjectData, o => o == find);
                if (found > -1)
                {
                    index = (ushort)found;
                    direction = Direction8.From(start.Attribute("Direction"));
                    return true;
                }
            }
            index = 0;
            direction = null;
            return false;
        }
    }
}
