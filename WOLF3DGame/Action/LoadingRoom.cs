﻿using Godot;
using System.Threading;
using WOLF3D.WOLF3DGame.Menu;
using WOLF3D.WOLF3DGame.OPL;

namespace WOLF3D.WOLF3DGame.Action
{
    public class LoadingRoom : Room
    {
        public LoadingRoom(ushort mapNumber = 0)
            : this(mapNumber, Settings.Episode, Settings.Difficulty) { }

        public LoadingRoom(ushort mapNumber, byte episode, byte difficulty)
        {
            Name = "LoadingRoom for map " + mapNumber;
            Episode = episode;
            Difficulty = difficulty;
            MapNumber = mapNumber;
            AddChild(ARVROrigin = new ARVROrigin());
            ARVROrigin.AddChild(ARVRCamera = new ARVRCamera()
            {
                Current = true,
            });
            ARVROrigin.AddChild(LeftController = new ARVRController()
            {
                ControllerId = 1,
            });
            ARVROrigin.AddChild(RightController = new ARVRController()
            {
                ControllerId = 2,
            });
            if (Assets.LoadingPic is ImageTexture pic && pic != null)
            {
                ARVRCamera.AddChild(new MeshInstance()
                {
                    Mesh = new QuadMesh()
                    {
                        Size = new Vector2(pic.GetWidth() * Assets.PixelWidth, pic.GetHeight() * Assets.PixelHeight),
                    },
                    MaterialOverride = new SpatialMaterial()
                    {
                        AlbedoTexture = pic,
                        FlagsUnshaded = true,
                        FlagsDoNotReceiveShadows = true,
                        FlagsDisableAmbientLight = true,
                        FlagsTransparent = false,
                        ParamsCullMode = SpatialMaterial.CullMode.Back,
                        ParamsSpecularMode = SpatialMaterial.SpecularMode.Disabled,
                    },
                    Transform = new Transform(Basis.Identity, Vector3.Forward * pic.GetWidth() * Assets.PixelWidth),
                });

                System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(ThreadProc));
                thread.IsBackground = true;
                thread.Start();
            }
        }
        public byte Difficulty { get; set; }
        public byte Episode { get; set; }
        public ushort MapNumber { get; set; }
        public ActionRoom ActionRoom { get; set; }

        public void ThreadProc()
        {
            ActionRoom = new ActionRoom()
            {
                Difficulty = Difficulty,
                Episode = Episode,
                MapNumber = MapNumber,
            };
            if (Main.NextLevelStats != null)
                ActionRoom.StatusBar.Set(Main.NextLevelStats);
            if (ActionRoom.StatusBar["Floor"] is StatusNumber floorNumber)
                floorNumber.Value = (uint)(MapNumber + 1);
        }

        public override void Enter()
        {
            base.Enter();
            Main.Color = Assets.Palette[Assets.Maps[MapNumber].Border];
            SoundBlaster.Song = Assets.AudioT.Songs[Assets.Maps[MapNumber].Song];
        }

        public override void _Process(float delta)
        {
            if (ActionRoom != null)
                Main.Room = Main.ActionRoom = ActionRoom;
        }
    }
}
