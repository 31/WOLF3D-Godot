﻿using Godot;
using System.Xml.Linq;

namespace WOLF3D.WOLF3DGame.Menu
{
    public class Target2D : Node2D, ITarget
    {
        public virtual bool IsIn(Vector2 vector2) => IsInLocal(vector2 - Position);
        public virtual bool IsIn(float x, float y) => IsInLocal(x - Position.x, y - Position.y);
        public virtual bool IsInLocal(Vector2 vector2) => IsInLocal(vector2.x, vector2.y);
        public virtual bool IsInLocal(float x, float y) => x + Offset.x >= 0 && y + Offset.y >= 0 && x + Offset.x < Size.x && y + Offset.y < Size.y;
        public virtual bool IsIn(Vector3 vector3) => IsIn(Assets.Vector2(vector3));
        public virtual bool IsIn(float x, float y, float z) => IsIn(x, z);
        public virtual bool IsInLocal(Vector3 vector3) => IsInLocal(Assets.Vector2(vector3));
        public virtual bool IsInLocal(float x, float y, float z) => IsInLocal(x, z);
        public virtual Vector2 Size { get; set; } = Vector2.Zero;
        public virtual Vector2 Offset { get; set; } = Vector2.Zero;
        public virtual XElement XML { get; set; } = null;
    }
}
