using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RTS1
{
#if WINDOWS || LINUX
    public static class Program
    {
        // Game timing stuff
        public static readonly Game1 Game = new Game1();
        public static ContentManager Content => Game.Content;

        public static KeyboardState Keyboard;
        public static KeyboardState LastKeyboard;
        public static MouseState Mouse;

        public static ControllerState Controller1;
        public static ControllerState Controller2;
        public static ControllerState Controller3;
        public static ControllerState Controller4;

        public static GameTime Gt;

        // Blur stuff
        public static bool Blur = false;
        public static Position BlurOrigin = new Position(0, 0);
        public static float BlurStrength = 12;
        public static int BlurOpacity = 150;

        [STAThread]
        static void Main()
        {
            Game.Run();
        }

        public static List<PlayerIndex> ConnectedPlayerIndecies()
        {
            List<PlayerIndex> r = new List<PlayerIndex>();
            if (Controller1.IsConnected) { r.Add(PlayerIndex.One); }
            if (Controller2.IsConnected) { r.Add(PlayerIndex.Two); }
            if (Controller3.IsConnected) { r.Add(PlayerIndex.Three); }
            if (Controller4.IsConnected) { r.Add(PlayerIndex.Four); }
            return r;
        }
        

        public static int ConnectedPlayers()
        {
            int r = 0;
            r += Controller1.IsConnected ? 1 : 0;
            r += Controller2.IsConnected ? 1 : 0;
            r += Controller3.IsConnected ? 1 : 0;
            r += Controller4.IsConnected ? 1 : 0;
            return r;
        }
    }
#endif
}
