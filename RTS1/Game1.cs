using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;

using RTS1_Data;
using RTS1.MonoGame;
using RTS1.MonoGame.GameObjects;

namespace RTS1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        public SpriteBatch SpriteBatch { get; private set; }

        public List<GameObject> TerrainTiles = new List<GameObject>();

        public List<GameObject> TerrainButtons = new List<GameObject>();

        public List<GameObject> Objects = new List<GameObject>();
        
        public List<GameObject> Overlay = new List<GameObject>();

        public List<GameObject> UI = new List<GameObject>();

        public List<OwnershipObject> Ownerships = new List<OwnershipObject>();

        //public Queue<GameObject> AddObjects = new Queue<GameObject>();
        //public Queue<GameObject> AddOverlay = new Queue<GameObject>();
        //public Queue<GameObject> AddUI = new Queue<GameObject>();
        
        //public Queue<GameObject> RemoveObjects = new Queue<GameObject>();
        //public Queue<GameObject> RemoveOverlay = new Queue<GameObject>();
        //public Queue<GameObject> RemoveUI = new Queue<GameObject>();

        public Position Camera = new Position(10, 10);

        public double CameraZoom = 1;
        
        public Position SCREEN = new Position(800, 600);
        
        #region Game Variables

        #region Intro Variables
        private FadeInFadeOut _intro;
        private Screen _currentScreen = Screen.Intro;
        private Color _backgroundColor = Color.Black;
        #endregion

        #region Menu Variables
        private Button _playButton;
        #endregion

        #region Charachter Select Variables
        private Sprite _charSelBg1;
        private Sprite _charSelBg2;
        private Sprite _charSelFg1;

        private Sprite _charSelP1Marker;
        private Sprite _charSelP2Marker;
        private Sprite _charSelP3Marker;
        private Sprite _charSelP4Marker;

        private Position _p00 = new Position(0, 0);
        private Position _p02 = new Position(373, 289);
        private Position _p03 = new Position(373, 156);
        private Position _p04 = new Position(723, 422);
        private Position _p05 = new Position(723, 289);
        private Position _p06 = new Position(373, 422);
        private Position _p07 = new Position(723, 156);

        private long _player1LastSelectionUpdate = 0;
        private long _player2LastSelectionUpdate = 0;
        private long _player3LastSelectionUpdate = 0;
        private long _player4LastSelectionUpdate = 0;

        private FourDirectionNode _player1SelectionNode = null;
        private FourDirectionNode _player2SelectionNode = null;
        private FourDirectionNode _player3SelectionNode = null;
        private FourDirectionNode _player4SelectionNode = null;
        private FourDirectionNode _prev_player1SelectionNode = null;
        private FourDirectionNode _prev_player2SelectionNode = null;
        private FourDirectionNode _prev_player3SelectionNode = null;
        private FourDirectionNode _prev_player4SelectionNode = null;
        private FourDirectionNode _n01 = new FourDirectionNode("Quit");
        private FourDirectionNode _n02 = new FourDirectionNode("Blue");
        private FourDirectionNode _n03 = new FourDirectionNode("Red");
        private FourDirectionNode _n04 = new FourDirectionNode("Yellow");
        private FourDirectionNode _n05 = new FourDirectionNode("Green");
        private FourDirectionNode _n06 = new FourDirectionNode("Gray");
        private FourDirectionNode _n07 = new FourDirectionNode("White");

        private List<PlayerNodeDataContainer> _n02l = new List<PlayerNodeDataContainer>();
        private List<PlayerNodeDataContainer> _n03l = new List<PlayerNodeDataContainer>();
        private List<PlayerNodeDataContainer> _n04l = new List<PlayerNodeDataContainer>();
        private List<PlayerNodeDataContainer> _n05l = new List<PlayerNodeDataContainer>();
        private List<PlayerNodeDataContainer> _n06l = new List<PlayerNodeDataContainer>();
        private List<PlayerNodeDataContainer> _n07l = new List<PlayerNodeDataContainer>();
        
        private List<FourDirectionNode> _selectionNodes = new List<FourDirectionNode>();

        private Character _p1Char = Character.None;
        private Character _p2Char = Character.None;
        private Character _p3Char = Character.None;
        private Character _p4Char = Character.None;

        private int _playersMadeDescision = 0;

        #endregion

        #region Battle Variables
        private TileMap _map;
        public double Gravity = 0.2;
        #endregion

        #region Debug Variables
        private bool _keyDebug = false;
        private int _dbgPlayer = 0;
        private bool _drawColliders = false;
        private TestPlayer _testp1;
        private TestPlayer _testp2;
        private TestPlayer _testp3;
        private TestPlayer _testp4;
        #endregion

        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = SCREEN.X;
            _graphics.PreferredBackBufferHeight = SCREEN.Y;
            Content.RootDirectory = "Content";
            
            IsMouseVisible = true;
        }
        
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            DataLoader.Initialize();

            _map = DataLoader.AddTilemap(@"C\Maps\testing_map", @"C\Maps\default_setting");
            _testp1 = new RedPlayer("#player1", new Position(200,200), @"C\Chars\Red\w_0_1");
            _testp1.FollowCamera = true;
            _testp1.AssociatedPlayer = PlayerIndex.One;
            _testp1.Initialize();
            _testp2 = new TestPlayer("#player2", 200, 220, @"C\Tiles\blue");
            _testp2.FollowCamera = true;
            _testp2.AssociatedPlayer = PlayerIndex.Two;
            _testp2.Initialize();
            _testp3 = new TestPlayer("#player3", 200, 220, @"C\Tiles\yellow");
            _testp3.FollowCamera = true;
            _testp3.AssociatedPlayer = PlayerIndex.Two;
            _testp3.Initialize();
            _testp4 = new TestPlayer("#player4", 200, 220, @"C\Tiles\red");
            _testp4.FollowCamera = true;
            _testp4.AssociatedPlayer = PlayerIndex.Two;
            _testp4.Initialize();


            _intro = new FadeInFadeOut("intro_text",400,300,@"C\Title");
            _playButton = new Button("play_button", 400, 300, @"C\UI\PlayButton");
            _charSelBg1 = new Sprite("bg_1", 400, 300, @"C\UI\CharSelBg1");
            _charSelBg2 = new Sprite("bg_2", 400, 300, @"C\UI\CharSelBg2");
            _charSelFg1 = new Sprite("fg_1", 400, 300, @"C\UI\CharSelFg1");
            _charSelP1Marker = new Sprite("cs_p1m", 0, 0, @"C\UI\CharSelP1Marker");
            _charSelP2Marker = new Sprite("cs_p2m", 0, 0, @"C\UI\CharSelP2Marker");
            _charSelP3Marker = new Sprite("cs_p3m", 0, 0, @"C\UI\CharSelP3Marker");
            _charSelP4Marker = new Sprite("cs_p4m", 0, 0, @"C\UI\CharSelP4Marker");
            
            Program.Controller1 = new ControllerState(PlayerIndex.One);
            Program.Controller2 = new ControllerState(PlayerIndex.Two);
            Program.Controller3 = new ControllerState(PlayerIndex.Three);
            Program.Controller4 = new ControllerState(PlayerIndex.Four);

            _n03.Set(null, _n02, null, _n07);
            _n07.Set(null, _n05, _n03, null);
            _n02.Set(_n03, _n06, null, _n05);
            _n05.Set(_n07, _n04, _n02, null);
            _n06.Set(_n02, null, null, _n04);
            _n04.Set(_n05, null, _n06, null);
            _selectionNodes.Add(_n03);
            _selectionNodes.Add(_n07);
            _selectionNodes.Add(_n02);
            _selectionNodes.Add(_n05);
            _selectionNodes.Add(_n04);
            _selectionNodes.Add(_n06);
        
            Objects.Add(_intro);
            //WaveObject wo = new WaveObject("#wave1",new Position(100,100), 20, 200, 5, 0.005);
            //WaveObject wo2 = new WaveObject("#wave1", new Position(100, 100), 20, 200, 5, 0.005);
            //wo.CreateWave(new string[] { @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\head" });
            //wo2.CreateWave(new string[] { @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\stick", @"C\Chars\Yellow\GoldenSerpents\head" },Math.PI);
            //Objects.Add(wo);
            //Objects.Add(wo2);
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected Animation GetAnimation(string name)
        {
            return DataLoader.GetAnimation(name);
        }

        protected override void Update(GameTime gameTime)
        {
            Timing.UpdateTiming.Update();
            AudioController.Update(Timing.UpdateTiming.Dt);

            Program.Keyboard = Keyboard.GetState();
            Program.Mouse = Mouse.GetState();
            Program.Gt = gameTime;
            
            if (Program.Keyboard.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            List<GameObject> garbage = new List<GameObject>();
            List<OwnershipObject> removedOwnerships = new List<OwnershipObject>();

            foreach (GameObject obj in Objects)
            {
                if(obj.Enabled)
                {
                    obj.Update(Timing.UpdateTiming.Dt);
                } else
                {
                    garbage.Add(obj);
                }
            }
            foreach (GameObject obj in Overlay)
            {
                if (obj.Enabled)
                {
                    obj.Update(Timing.UpdateTiming.Dt);
                }
                else
                {
                    garbage.Add(obj);
                }
            }
            foreach (GameObject obj in UI)
            {
                if (obj.Enabled)
                {
                    obj.Update(Timing.UpdateTiming.Dt);
                }
                else
                {
                    garbage.Add(obj);
                }
            }
            
            foreach (GameObject obj in garbage)
            {
                Objects.Remove(obj);
                Overlay.Remove(obj);
                UI.Remove(obj);
            }

            foreach (OwnershipObject ow in Ownerships)
            {
                if (!Objects.Contains(ow.Object) && !UI.Contains(ow.Object) && !Overlay.Contains(ow.Object))
                {
                    removedOwnerships.Add(ow);
                }
            }

            foreach (OwnershipObject ow in removedOwnerships)
            {
                Ownerships.Remove(ow);
            }

            if (_intro.ActionFinished && _currentScreen == Screen.Intro)
            {
                Console.WriteLine("Menu");
                _currentScreen = Screen.Menu;
                _playButton.AutoFitCollider = true;
                _playButton.WrapInCollider(true);
                Objects.Add(_playButton);
                Objects.Remove(_intro);
            }

            if (((_playButton.Clicked&&_keyDebug) || WasReleased(Buttons.A))&& _currentScreen == Screen.Menu)
            {
                _currentScreen = Screen.CharachterSelect;
                Console.WriteLine("CharachterSelect");
                Objects.Add(_charSelBg1);
                Objects.Add(_charSelBg2);
                Objects.Add(_charSelFg1);
                if(_keyDebug)
                {
                    Objects.Add(_charSelP1Marker);
                    Objects.Add(_charSelP2Marker);
                    Objects.Add(_charSelP3Marker);
                    Objects.Add(_charSelP4Marker);
                } else
                {
                    if(Program.Controller1.IsConnected)
                    {
                        Objects.Add(_charSelP1Marker);
                        _player1SelectionNode = _selectionNodes.RandomItem();
                    }
                    if (Program.Controller2.IsConnected)
                    {
                        Objects.Add(_charSelP2Marker);
                        _player2SelectionNode = _selectionNodes.RandomItem();
                    }
                    if (Program.Controller3.IsConnected)
                    {
                        Objects.Add(_charSelP3Marker);
                        _player3SelectionNode = _selectionNodes.RandomItem();
                    }
                    if (Program.Controller4.IsConnected)
                    {
                        Objects.Add(_charSelP4Marker);
                        _player4SelectionNode = _selectionNodes.RandomItem();
                    }
                }
                Objects.Remove(_playButton);
            }

            if (_currentScreen == Screen.CharachterSelect)
            {
                if(_playersMadeDescision==Program.ConnectedPlayers())
                {
                    _currentScreen = Screen.Battle;
                    Console.WriteLine("Battle");
                    Objects.Remove(_charSelBg1);
                    Objects.Remove(_charSelBg2);
                    Objects.Remove(_charSelFg1);
                    Objects.Remove(_charSelP1Marker);
                    Objects.Remove(_charSelP2Marker);
                    Objects.Remove(_charSelP3Marker);
                    Objects.Remove(_charSelP4Marker);
                    BattleStart();
                }
                if (Program.Controller1.Connected()) 
                {
                    Objects.Add(_charSelP1Marker);
                    _player1SelectionNode = _selectionNodes.RandomItem();
                    _player1LastSelectionUpdate = GetMs();
                }
                if (Program.Controller2.Connected())
                {
                    Objects.Add(_charSelP2Marker);
                    _player2SelectionNode = _selectionNodes.RandomItem();
                    _player2LastSelectionUpdate = GetMs();
                }
                if (Program.Controller3.Connected())
                {
                    Objects.Add(_charSelP3Marker);
                    _player3SelectionNode = _selectionNodes.RandomItem();
                    _player3LastSelectionUpdate = GetMs();
                }
                if (Program.Controller4.Connected())
                {
                    Objects.Add(_charSelP4Marker);
                    _player4SelectionNode = _selectionNodes.RandomItem();
                    _player4LastSelectionUpdate = GetMs();
                }

                if (Program.Controller1.Disconnected())
                {
                    Objects.Remove(_charSelP1Marker);
                    _player1SelectionNode = null;
                }
                if (Program.Controller2.Disconnected())
                {
                    Objects.Remove(_charSelP2Marker);
                    _player2SelectionNode = null;
                }
                if (Program.Controller3.Disconnected())
                {
                    Objects.Remove(_charSelP3Marker);
                    _player3SelectionNode = null;
                }
                if (Program.Controller4.Disconnected())
                {
                    Objects.Remove(_charSelP4Marker);
                    _player4SelectionNode = null;
                }

                if (_keyDebug&&Program.Keyboard.WasKeyPressed(Keys.Tab))
                {
                    _dbgPlayer += 1;
                    _dbgPlayer %= 4;
                    Console.WriteLine($"Current Player : {_dbgPlayer}");
                }
                if ((Program.Controller1.IsConnected || _keyDebug) && _player1SelectionNode != null)
                {
                    _charSelP1Marker.Position = GetMarkerPositionFromNode(_player1SelectionNode);
                }
                if ((Program.Controller2.IsConnected || _keyDebug) && _player2SelectionNode != null)
                {
                    _charSelP2Marker.Position = GetMarkerPositionFromNode(_player2SelectionNode);
                }
                if ((Program.Controller3.IsConnected || _keyDebug) && _player3SelectionNode != null)
                {
                    _charSelP3Marker.Position = GetMarkerPositionFromNode(_player3SelectionNode);
                }
                if ((Program.Controller4.IsConnected || _keyDebug) && _player4SelectionNode != null)
                {
                    _charSelP4Marker.Position = GetMarkerPositionFromNode(_player4SelectionNode);
                }
                _n02l = GetEquivalentPlayerNodesInLastAccessOrder(_n02);
                _n03l = GetEquivalentPlayerNodesInLastAccessOrder(_n03);
                _n04l = GetEquivalentPlayerNodesInLastAccessOrder(_n04);
                _n05l = GetEquivalentPlayerNodesInLastAccessOrder(_n05);
                _n06l = GetEquivalentPlayerNodesInLastAccessOrder(_n06);
                _n07l = GetEquivalentPlayerNodesInLastAccessOrder(_n07);

                List<List<PlayerNodeDataContainer>> nds = new List<List<PlayerNodeDataContainer>>();

                nds.Add(_n02l);
                nds.Add(_n03l);
                nds.Add(_n04l);
                nds.Add(_n05l);
                nds.Add(_n06l);
                nds.Add(_n07l);
                foreach(List<PlayerNodeDataContainer> nl in nds)
                {
                    if (nl.Count > 0)
                    {
                        int idp1 = nl.IndexOf(PlayerIndex.One);
                        int idp2 = nl.IndexOf(PlayerIndex.Two);
                        int idp3 = nl.IndexOf(PlayerIndex.Three);
                        int idp4 = nl.IndexOf(PlayerIndex.Four);
                        //Console.WriteLine($"{idp1}\t{idp2}\t{idp3}\t{idp4}");
                        Position p1ps = _charSelP1Marker.Position;
                        Position p2ps = _charSelP2Marker.Position;
                        Position p3ps = _charSelP3Marker.Position;
                        Position p4ps = _charSelP4Marker.Position;
                        _charSelP1Marker.Position = idp1 != -1 ? new Position(p1ps.X - (idp1 * 48), p1ps.Y) : p1ps;
                        _charSelP2Marker.Position = idp2 != -1 ? new Position(p2ps.X - (idp2 * 48), p2ps.Y) : p2ps;
                        _charSelP3Marker.Position = idp3 != -1 ? new Position(p3ps.X - (idp3 * 48), p3ps.Y) : p3ps;
                        _charSelP4Marker.Position = idp4 != -1 ? new Position(p4ps.X - (idp4 * 48), p4ps.Y) : p4ps;
                    }
                }
            }

            if (_currentScreen!=Screen.Intro)
            {
                if(_keyDebug)
                {
                    DoControllerActions(Program.Controller1);
                } else
                {
                    DoControllerActions(Program.Controller1);
                    DoControllerActions(Program.Controller2);
                    DoControllerActions(Program.Controller3);
                    DoControllerActions(Program.Controller4);
                }
            }

            if (_currentScreen==Screen.Battle)
            {
                Camera = new Position(400 - _testp1.Position.X, 300 - _testp1.Position.Y);
            }

            _prev_player1SelectionNode = _player1SelectionNode;
            _prev_player2SelectionNode = _player2SelectionNode;
            _prev_player3SelectionNode = _player3SelectionNode;
            _prev_player4SelectionNode = _player4SelectionNode;
            Program.Controller1.UpdateState();
            Program.Controller2.UpdateState();
            Program.Controller3.UpdateState();
            Program.Controller4.UpdateState();
            Program.LastKeyboard = Program.Keyboard;
            base.Update(gameTime);
        }

        private TestPlayer PlayerFromCharacter(Character c)
        {
            return null;
        }

        private void DoControllerActions(ControllerState controller)
        {
            if (_keyDebug)
            {
                if (_player1SelectionNode == null)
                {
                    _player1SelectionNode = _selectionNodes.RandomItem();
                    _player1LastSelectionUpdate = GetMs();
                }
                if (_player2SelectionNode == null)
                {
                    _player2SelectionNode = _selectionNodes.RandomItem();
                    _player2LastSelectionUpdate = GetMs();
                }
                if (_player3SelectionNode == null)
                {
                    _player3SelectionNode = _selectionNodes.RandomItem();
                    _player3LastSelectionUpdate = GetMs();
                }
                if (_player4SelectionNode == null)
                {
                    _player4SelectionNode = _selectionNodes.RandomItem();
                    _player4LastSelectionUpdate = GetMs();
                }
                Direction sent = Direction.None;
                if (Program.Keyboard.WasKeyPressed(Keys.Up))
                {
                    sent = Direction.Up;
                }
                if (Program.Keyboard.WasKeyPressed(Keys.Down))
                {
                    sent = Direction.Down;
                }
                if (Program.Keyboard.WasKeyPressed(Keys.Right))
                {
                    sent = Direction.Right;
                }
                if (Program.Keyboard.WasKeyPressed(Keys.Left))
                {
                    sent = Direction.Left;
                }
                switch (_dbgPlayer)
                {
                    case 0:
                        KeyValuePair<FourDirectionNode, bool> p1sn = _player1SelectionNode.MeaningfulGet(sent);
                        _player1SelectionNode = p1sn.Key;
                        if (p1sn.Value)
                        {
                            _player1LastSelectionUpdate = GetMs();
                        }
                        break;
                    case 1:
                        KeyValuePair<FourDirectionNode, bool> p2sn = _player2SelectionNode.MeaningfulGet(sent);
                        _player2SelectionNode = p2sn.Key;
                        if (p2sn.Value)
                        {
                            _player2LastSelectionUpdate = GetMs();
                        }
                        break;
                    case 2:
                        KeyValuePair<FourDirectionNode, bool> p3sn = _player3SelectionNode.MeaningfulGet(sent);
                        _player3SelectionNode = p3sn.Key;
                        if (p3sn.Value)
                        {
                            _player3LastSelectionUpdate = GetMs();
                        }
                        break;
                    case 3:
                        KeyValuePair<FourDirectionNode, bool> p4sn = _player4SelectionNode.MeaningfulGet(sent);
                        _player4SelectionNode = p4sn.Key;
                        if (p4sn.Value)
                        {
                            _player4LastSelectionUpdate = GetMs();
                        }
                        break;
                }
            }
            if (!controller.IsConnected) { return; }
            if (_currentScreen == Screen.CharachterSelect)
            {
                Character chr = Character.None;
                switch (controller.AssociatedPlayer)
                {
                    case PlayerIndex.One:
                        chr = _p1Char;
                        // insert create player character here
                        break;
                    case PlayerIndex.Two:
                        chr = _p2Char;
                        break;
                    case PlayerIndex.Three:
                        chr = _p3Char;
                        break;
                    case PlayerIndex.Four:
                        chr = _p4Char;
                        break;
                }
                if (chr != Character.None)
                {
                    if (controller.IsButtonDown(Buttons.B))
                    {
                        Console.WriteLine($"Deselected {chr}");
                        _playersMadeDescision -= 1;
                        switch (controller.AssociatedPlayer)
                        {
                            case PlayerIndex.One:
                                _p1Char = Character.None;
                                break;
                            case PlayerIndex.Two:
                                _p2Char = Character.None;
                                break;
                            case PlayerIndex.Three:
                                _p3Char = Character.None;
                                break;
                            case PlayerIndex.Four:
                                _p4Char = Character.None;
                                break;
                        }
                    }
                }
                else
                {
                    if (controller.WasButtonPressed(Buttons.A))
                    {
                        Character c;
                        switch (controller.AssociatedPlayer)
                        {
                            case PlayerIndex.One:
                                c = CharacterFromNode(_player1SelectionNode);
                                if (!AlreadySelectedCharachter(c))
                                {
                                    Console.WriteLine("Selected " + c);
                                    _playersMadeDescision += 1;
                                     _p1Char = c;
                                    _testp1.Character = c;
                                }
                                break;
                            case PlayerIndex.Two:
                                c = CharacterFromNode(_player2SelectionNode);
                                {
                                    Console.WriteLine("Selected " + c);
                                    _playersMadeDescision += 1;
                                    _p2Char = c;
                                    _testp2.Character = c;
                                }
                                break;
                            case PlayerIndex.Three:
                                c = CharacterFromNode(_player3SelectionNode);
                                {
                                    Console.WriteLine("Selected " + c);
                                    _playersMadeDescision += 1;
                                    _p3Char = c;
                                    _testp3.Character = c;
                                }
                                break;
                            case PlayerIndex.Four:
                                c = CharacterFromNode(_player4SelectionNode);
                                {
                                    Console.WriteLine("Selected " + c);
                                    _playersMadeDescision += 1;
                                    _p4Char = c;
                                    _testp4.Character = c;
                                }
                                break;
                        }
                    }
                    Direction sent = Direction.None;
                    if (controller.LeftThumbstickDirectionChanged(Direction.Right) || controller.WasButtonPressed(Buttons.DPadRight))
                    {
                        sent = Direction.Right;
                    }
                    if (controller.LeftThumbstickDirectionChanged(Direction.Left) || controller.WasButtonPressed(Buttons.DPadLeft))
                    {
                        sent = Direction.Left;
                    }
                    if (controller.LeftThumbstickDirectionChanged(Direction.Up) || controller.WasButtonPressed(Buttons.DPadUp))
                    {
                        sent = Direction.Up;
                    }
                    if (controller.LeftThumbstickDirectionChanged(Direction.Down) || controller.WasButtonPressed(Buttons.DPadDown))
                    {
                        sent = Direction.Down;
                    }
                    switch (controller.AssociatedPlayer)
                    {
                        case PlayerIndex.One:
                            KeyValuePair<FourDirectionNode, bool> p1sn = _player1SelectionNode.MeaningfulGet(sent);
                            _player1SelectionNode = p1sn.Key;
                            if (p1sn.Value)
                            {
                                _player1LastSelectionUpdate = GetMs();
                            }
                            break;
                        case PlayerIndex.Two:
                            KeyValuePair<FourDirectionNode, bool> p2sn = _player2SelectionNode.MeaningfulGet(sent);
                            _player2SelectionNode = p2sn.Key;
                            if (p2sn.Value)
                            {
                                _player2LastSelectionUpdate = GetMs();
                            }
                            break;
                        case PlayerIndex.Three:
                            KeyValuePair<FourDirectionNode, bool> p3sn = _player3SelectionNode.MeaningfulGet(sent);
                            _player3SelectionNode = p3sn.Key;
                            if (p3sn.Value)
                            {
                                _player3LastSelectionUpdate = GetMs();
                            }
                            break;
                        case PlayerIndex.Four:
                            KeyValuePair<FourDirectionNode, bool> p4sn = _player4SelectionNode.MeaningfulGet(sent);
                            _player4SelectionNode = p4sn.Key;
                            if (p4sn.Value)
                            {
                                _player4LastSelectionUpdate = GetMs();
                            }
                            break;
                    }
                }
            }
            if (_currentScreen == Screen.Battle)
            {
                if(controller.WasButtonPressed(Buttons.DPadDown))
                {
                    CameraZoom -= 0.1;
                }
                if (controller.WasButtonPressed(Buttons.DPadUp))
                {
                    CameraZoom += 0.1;
                }
                if (controller.AssociatedPlayer==_testp1.AssociatedPlayer)
                {
                    _testp1.Update(Timing.UpdateTiming.Dt, GetDirectionalLimiters(_testp1), controller);
                }
                else if (controller.AssociatedPlayer == _testp2.AssociatedPlayer)
                {
                    _testp2.Update(Timing.UpdateTiming.Dt, GetDirectionalLimiters(_testp2), controller);
                }
            }
        }
        
        private void BattleStart()
        {
            _backgroundColor = Color.CornflowerBlue;
            _testp1 = NewPlayerFromBasePlayer(_testp1);
            _testp2 = NewPlayerFromBasePlayer(_testp2);
            _testp3 = NewPlayerFromBasePlayer(_testp3);
            _testp4 = NewPlayerFromBasePlayer(_testp4);
            Console.WriteLine("Battle Started");
            Objects.Add(_map);
            _map.AddSubCollidersToObjectsList();
            Objects.Add(_testp1);
            UI.Add(_testp1.HealthBar);
            Objects.Add(_testp2);
            UI.Add(_testp2.HealthBar);
        }

        #region Helper Methods
        private bool WasPressed(Buttons b)
        {
            return Program.Controller1.WasButtonPressed(b) || 
                Program.Controller2.WasButtonPressed(b) || 
                Program.Controller3.WasButtonPressed(b) || 
                Program.Controller4.WasButtonPressed(b);
        }

        private TestPlayer NewPlayerFromBasePlayer(TestPlayer c)
        {
            TestPlayer r = c;
            bool init = false;
            switch (c.Character)
            {
                case Character.Blue:
                    break;
                case Character.Green:
                    break;
                case Character.Grey:
                    break;
                case Character.Red:
                    r = new RedPlayer($"#player{c.AssociatedPlayer}", new Position(200, 200), @"C\Chars\Red\w_0_1");
                    init = true;
                    break;
                case Character.White:
                    break;
                case Character.Yellow:
                    r = new YellowPlayer($"#player{c.AssociatedPlayer}", new Position(200, 200), @"C\Chars\Yellow\w_0_0");
                    init = true;
                    break;
            }
            if(init!=false)
            {
                r.FollowCamera = true;
                r.AssociatedPlayer = c.AssociatedPlayer;
                r.Initialize();
            }
            return r;
        }

        public List<TestPlayer> TouchingPlayers(GameObject go)
        {
            List<TestPlayer> ret = new List<TestPlayer>();
            foreach (GameObject obj in Objects)
            {
                if (obj.Name.Contains("#player") && obj.CollidesWith(go))
                {
                    ret.Add((TestPlayer)obj);
                }
            }
            return ret;
        }

        public GameObject TouchingPlayer(GameObject go)
        {
            foreach (GameObject obj in Objects)
            {
                if (obj.Name.Contains("#player") && obj.CollidesWith(go))
                {
                    return obj;
                }
            }
            return null;
        }

        public GameObject InsideWall(GameObject go)
        {
            for (int y = 0; y < _map.array.GetLength(0); y++)
            {
                for (int x = 0; x < _map.array.GetLength(1); x++)
                {
                    var obj = _map.array[y, x];
                    if (obj.Name.Contains("#WALL") && !obj.Tag.EndsWith(@"empty") && obj.CollidesWith(go) )
                    {
                        return obj;
                    }
                }
            }
            return null;
        }

        private List<Direction> GetDirectionalLimiters(GameObject go)
        {
            List<Direction> ret = new List<Direction>();
            foreach (GameObject obj in Objects)
            {
                if (obj.Tag.Contains("Collider") && obj.CollidesWith(go))
                {
                    switch (obj.Tag.Replace("Collider", ""))
                    {
                        case "#top":
                            ret.Add(Direction.Down);
                            break;
                        case "#bot":
                            ret.Add(Direction.Up);
                            break;
                        case "#lft":
                            ret.Add(Direction.Right);
                            break;
                        case "#rit":
                            ret.Add(Direction.Left);
                            break;
                    }
                }
            }
            return ret;
        }

        private bool TouchingDirectionalLimiter(GameObject go, Direction d)
        {
            foreach (GameObject obj in Objects)
            {
                if(obj.Tag.Contains("Collider")&&obj.CollidesWith(go))
                {
                    switch(obj.Tag.Replace("Collider",""))
                    {
                        case "#top":
                            return Direction.Down == d;
                        case "#bot":
                            return Direction.Up == d;
                        case "#lft":
                            return Direction.Right == d;
                        case "#rit":
                            return Direction.Left == d;
                    }
                }
            }
            return false;
        }

        private bool WasReleased(Buttons b)
        {
            return Program.Controller1.WasButtonReleased(b) ||
                Program.Controller2.WasButtonReleased(b) ||
                Program.Controller3.WasButtonReleased(b) ||
                Program.Controller4.WasButtonReleased(b);
        }

        private bool AlreadySelectedCharachter(Character c)
        {
            return c == _p1Char || c == _p2Char || c == _p3Char || c == _p4Char;
        }

        public TestPlayer ClocestCharachterInRange(TestPlayer t, int distance)
        {
            if (t.AssociatedPlayer != _testp1.AssociatedPlayer && t.Distance(_testp1) < distance)
            {
                return _testp1;
            }
            if (t.AssociatedPlayer != _testp2.AssociatedPlayer && t.Distance(_testp2) < distance)
            {
                return _testp2;
            }
            if (t.AssociatedPlayer != _testp3.AssociatedPlayer && t.Distance(_testp3) < distance)
            {
                return _testp3;
            }
            if (t.AssociatedPlayer != _testp4.AssociatedPlayer && t.Distance(_testp4) < distance)
            {
                return _testp4;
            }
            return null;
        }

        private Character CharacterFromNode(FourDirectionNode n)
        {
            if (n == _n02) { return Character.Blue; }
            if (n == _n03) { return Character.Red; }
            if (n == _n04) { return Character.Yellow; }
            if (n == _n05) { return Character.Green; }
            if (n == _n06) { return Character.Grey; }
            if (n == _n07) { return Character.White; }
            return Character.None;
        }

        private int PlayersOnNode(FourDirectionNode node)
        {
            int r = 0;
            r += _player1SelectionNode == node ? 1 : 0;
            r += _player2SelectionNode == node ? 1 : 0;
            r += _player3SelectionNode == node ? 1 : 0;
            r += _player4SelectionNode == node ? 1 : 0;
            return r;
        }
        
        private FourDirectionNode RelatedNode( ControllerState controller)
        {
            switch (controller.AssociatedPlayer)
            {
                case PlayerIndex.One:
                    return _player1SelectionNode;
                case PlayerIndex.Two:
                    return _player2SelectionNode;
                case PlayerIndex.Three:
                    return _player3SelectionNode;
                case PlayerIndex.Four:
                    return _player4SelectionNode;
            }
            return null;
        }

        private List<PlayerNodeDataContainer> GetEquivalentPlayerNodesInLastAccessOrder( FourDirectionNode node)
        {
            List<PlayerNodeDataContainer> ret = new List<PlayerNodeDataContainer>();
            if (_player1SelectionNode == node) { ret.Add(new PlayerNodeDataContainer(_player1LastSelectionUpdate, PlayerIndex.One)); }
            if (_player2SelectionNode == node) { ret.Add(new PlayerNodeDataContainer(_player2LastSelectionUpdate, PlayerIndex.Two)); }
            if (_player3SelectionNode == node) { ret.Add(new PlayerNodeDataContainer(_player3LastSelectionUpdate, PlayerIndex.Three)); }
            if (_player4SelectionNode == node) { ret.Add(new PlayerNodeDataContainer(_player4LastSelectionUpdate, PlayerIndex.Four)); }
            if (ret.Count>0)
            {
                ret.OrderBy(f => f.LastSelectionUpdate);
                ret.Reverse();   
            }
            return ret;
        }

        private long GetMs()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private Position GetMarkerPositionFromNode(FourDirectionNode node)
        {
            Position ret = _p00;
            ret = node == _n02 ? _p02 : ret;
            ret = node == _n03 ? _p03 : ret;
            ret = node == _n04 ? _p04 : ret;
            ret = node == _n05 ? _p05 : ret;
            ret = node == _n06 ? _p06 : ret;
            ret = node == _n07 ? _p07 : ret;
            return ret;
        }
        #endregion

        protected override void Draw(GameTime gameTime)
        {
            Timing.DrawTiming.Update();
            MouseState state_m = Mouse.GetState();
            Position mPos = new Position(state_m.X, state_m.Y);
            GraphicsDevice.Clear(_backgroundColor);
            
            SpriteBatch.Begin(blendState: BlendState.AlphaBlend);
            //Dictionary<string, int> disposition = new Dictionary<string, int>();
            foreach(GameObject obj in Objects)
            {
                if(obj.Tag.Contains("#map"))
                {
                    obj.Scale = (float)CameraZoom;
                    _testp1.Scale = (float)CameraZoom;
                    _testp2.Scale = (float)CameraZoom;
                    //((TileMap)obj).Draw(Timing.DrawTiming.Dt, Camera);
                    ((TileMap)obj).Draw(Timing.DrawTiming.Dt);
                }
                else
                {
                    if(obj.Name== "#drawnCollider"&&!_drawColliders)
                    {
                        continue;
                    }
                    obj.Draw(Timing.DrawTiming.Dt);
                }
                //if(disposition.ContainsKey(obj.Name))
                //{
                //    disposition[obj.Name] += 1;
                //} else
                //{
                //    disposition.Add(obj.Name, 1);
                //}
            }
            foreach (GameObject obj in Overlay)
            {
                obj.Draw(Timing.DrawTiming.Dt);
            }
            foreach (GameObject obj in UI)
            {
                if (obj.Tag.Contains("#map"))
                {
                    ((TileMap)obj).Draw(Timing.DrawTiming.Dt);
                }
                else
                {
                    if (obj.Name == "#drawnCollider" && !_drawColliders)
                    {
                        continue;
                    }
                    obj.Draw(Timing.DrawTiming.Dt);
                }
            }
            //Console.WriteLine("\nUpdated Objects:");
            //foreach (KeyValuePair<string, int> entry in disposition)
            //{
            //    Console.WriteLine($"{entry.Key} :\t{entry.Value}");
            //}
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
