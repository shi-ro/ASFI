using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RTS1.MonoGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public class ControllerState
    {
        public GamePadState CurrentState { get; private set; }
        public GamePadState PastState { get; private set; }
        public PlayerIndex AssociatedPlayer { get; private set; }
        public GamePadButtons Buttons { get; private set; }
        public GamePadDPad DPad { get; private set; }
        public bool IsConnected { get; private set; }
        public int PacketNumber { get; private set; }
        public GamePadThumbSticks ThumbSticks { get; private set; }
        public GamePadTriggers Triggers { get; private set; }

        public bool HasRightVibrationMotor { get; private set; }
        public bool HasLeftVibrationMotor { get; private set; }
        public bool HasRightTrigger { get; private set; }
        public bool HasLeftTrigger { get; private set; }
        public bool HasRightYThumbStick { get; private set; }
        public bool HasRightXThumbStick { get; private set; }
        public bool HasLeftYThumbStick { get; private set; }
        public bool HasLeftXThumbStick { get; private set; }
        public bool HasBigButton { get; private set; }
        public bool HasYButton { get; private set; }
        public bool HasXButton { get; private set; }
        public bool HasStartButton { get; private set; }
        public bool HasRightStickButton { get; private set; }
        public bool HasRightShoulderButton { get; private set; }
        public bool HasLeftStickButton { get; private set; }
        public bool HasLeftShoulderButton { get; private set; }
        public bool HasDPadUpButton { get; private set; }
        public bool HasDPadRightButton { get; private set; }
        public bool HasDPadLeftButton { get; private set; }
        public bool HasDPadDownButton { get; private set; }
        public bool HasBButton { get; private set; }
        public bool HasBackButton { get; private set; }
        public bool HasAButton { get; private set; }
        public bool HasVoiceSupport { get; private set; }
        public GamePadType GamePadType { get; private set; }

        public GamePadCapabilities Capabilities { get; private set; }

        public Direction LeftThumbstickDirection = Direction.None;
        public Direction LeftThumbstickDeltaDirection = Direction.None;
        public Direction RightThumbstickDirection = Direction.None;
        public Direction RightThumbstickDeltaDirection = Direction.None;

        public Vector2 LeftThumbstickDelta { get; private set; }
        public Vector2 RightThumbstickDelta { get; private set; }

        public double LeftThumbstickAngle { get; private set; }
        public double RightThumbstickAngle { get; private set; }
        public double LeftThumbstickDeltaAngle { get; private set; }
        public double RightThumbstickDeltaAngle { get; private set; }
        public double LeftTriggerPower { get; private set; }
        public double RightTriggerPower { get; private set; }

        private Direction _l = Direction.Left;
        private Direction _r = Direction.Right;
        private Direction _u = Direction.Up;
        private Direction _d = Direction.Down;
        private Direction _lu = Direction.UpLeft;
        private Direction _ld = Direction.DownLeft;
        private Direction _ru = Direction.UpRight;
        private Direction _rd = Direction.DownRight;
        private Direction _n = Direction.None;
        
        private Direction _pastLeftThumbstickDirection = Direction.None;
        private Direction _pastRightThumbstickDirection = Direction.None;
        private Direction _pastLeftThumbstickDeltaDirection = Direction.None;
        private Direction _pastRightThumbstickDeltaDirection = Direction.None;

        private bool _vibrating = false;
        private long _vibrationStartTime;
        private int _vibrationDuration;
        private float _vibrationRight;
        private float _vibrationLeft;

        public ControllerState(PlayerIndex player)
        {
            AssociatedPlayer = player;
            CurrentState = GamePad.GetState(player);
            PastState = GamePad.GetState(player);
            Capabilities = GamePad.GetCapabilities(player);

            UpdateVariables();
        }
        
        private void UpdateVariables()
        {
            _pastLeftThumbstickDeltaDirection = LeftThumbstickDeltaDirection;
            _pastRightThumbstickDeltaDirection = RightThumbstickDeltaDirection;
            _pastLeftThumbstickDirection = LeftThumbstickDirection;
            _pastRightThumbstickDirection = RightThumbstickDirection;

            Buttons = CurrentState.Buttons;
            DPad = CurrentState.DPad;
            IsConnected = CurrentState.IsConnected;
            PacketNumber = CurrentState.PacketNumber;
            ThumbSticks = CurrentState.ThumbSticks;
            Triggers = CurrentState.Triggers;

            HasBackButton = Capabilities.HasBackButton;
            HasBButton = Capabilities.HasBButton;
            HasBigButton = Capabilities.HasBigButton;
            HasDPadDownButton = Capabilities.HasDPadDownButton;
            HasDPadLeftButton = Capabilities.HasDPadLeftButton;
            HasDPadRightButton = Capabilities.HasDPadRightButton;
            HasDPadUpButton = Capabilities.HasDPadUpButton;
            HasLeftShoulderButton = Capabilities.HasLeftShoulderButton;
            HasLeftStickButton = Capabilities.HasLeftStickButton;
            HasLeftTrigger = Capabilities.HasLeftTrigger;
            HasLeftVibrationMotor = Capabilities.HasLeftVibrationMotor;
            HasLeftXThumbStick = Capabilities.HasLeftXThumbStick;
            HasLeftYThumbStick = Capabilities.HasLeftYThumbStick;
            HasRightShoulderButton = Capabilities.HasRightShoulderButton;
            HasRightStickButton = Capabilities.HasRightStickButton;
            HasRightTrigger = Capabilities.HasRightTrigger;
            HasRightVibrationMotor = Capabilities.HasRightVibrationMotor;
            HasRightXThumbStick = Capabilities.HasRightXThumbStick;
            HasRightYThumbStick = Capabilities.HasRightYThumbStick;
            HasStartButton = Capabilities.HasStartButton;
            HasVoiceSupport = Capabilities.HasVoiceSupport;
            HasXButton = Capabilities.HasXButton;
            HasYButton = Capabilities.HasYButton;

            if (HasLeftXThumbStick && HasLeftYThumbStick)
            {
                float x = ThumbSticks.Left.X;
                float y = ThumbSticks.Left.Y;
                bool lt = x < -0.5f;
                bool rt = x > 0.5f;
                bool ut = y > 0.5f;
                bool dt = y < -0.5f;
                LeftThumbstickDirection = _n;
                LeftThumbstickDirection = lt == rt ? _n : lt ? _l : rt ? _r : _n;
                LeftThumbstickDirection = ut == dt ? lt == rt ? _n : lt ? _l : rt ? _r : _n : lt && ut ? _lu : lt && dt ? _ld : rt && ut ? _ru : rt && dt ? _rd : lt == rt ? _n : lt ? _l : rt ? _r : _n;
                LeftThumbstickDirection = LeftThumbstickDirection == _n ? ut ? _u : dt ? _d : _n : LeftThumbstickDirection;
                LeftThumbstickAngle = Math.Atan2(y, x) * (180 / Math.PI) < 0 ? 360 + Math.Atan2(y, x) * (180 / Math.PI) : Math.Atan2(y, x) * (180 / Math.PI);
                float dx = CurrentState.ThumbSticks.Left.X-PastState.ThumbSticks.Left.X;
                float dy = CurrentState.ThumbSticks.Left.Y-PastState.ThumbSticks.Left.Y;
                LeftThumbstickDeltaAngle = Math.Atan2(dy, dx) * (180 / Math.PI) < 0 ? 360 - Math.Atan2(dy, dx) * (180 / Math.PI) : Math.Atan2(dy, dx) * (180 / Math.PI);
                bool dlt = dx < -0.5f;
                bool drt = dx > 0.5f;
                bool dut = dy > 0.5f;
                bool ddt = dy < -0.5f;
                LeftThumbstickDeltaDirection = _n;
                LeftThumbstickDeltaDirection = dlt == drt ? _n : dlt ? _l : drt ? _r : _n;
                LeftThumbstickDeltaDirection = dut == ddt ? dlt == drt ? _n : dlt ? _l : drt ? _r : _n : dlt && dut ? _lu : dlt && ddt ? _ld : drt && dut ? _ru : drt && ddt ? _rd : dlt == drt ? _n : dlt ? _l : drt ? _r : _n;
                LeftThumbstickDeltaDirection = LeftThumbstickDeltaDirection == _n ? ut ? _u : dt ? _d : _n : LeftThumbstickDeltaDirection;
                LeftThumbstickDelta = new Vector2(dx, dy);
            }
            if (HasRightXThumbStick && HasRightXThumbStick)
            {
                float x = ThumbSticks.Right.X;
                float y = ThumbSticks.Right.Y;
                bool lt = ThumbSticks.Right.X < -0.5f;
                bool rt = ThumbSticks.Right.X > 0.5f;
                bool ut = ThumbSticks.Right.Y > 0.5f;
                bool dt = ThumbSticks.Right.Y < -0.5f;
                RightThumbstickDirection = _n;
                RightThumbstickDirection = lt == rt ? _n : lt ? _l : rt ? _r : _n;
                RightThumbstickDirection = ut == dt ? lt == rt ? _n : lt ? _l : rt ? _r : _n : lt && ut ? _lu : lt && dt ? _ld : rt && ut ? _ru : rt && dt ? _rd : lt == rt ? _n : lt ? _l : rt ? _r : _n;
                RightThumbstickDirection = RightThumbstickDirection == _n ? ut ? _u : dt ? _d : _n : RightThumbstickDirection;
                RightThumbstickAngle = Math.Atan2(y, x) * (180 / Math.PI) < 0 ? 360 + Math.Atan2(y, x) * (180 / Math.PI) : Math.Atan2(y, x) * (180 / Math.PI);
                float dx = CurrentState.ThumbSticks.Right.X - PastState.ThumbSticks.Right.X;
                float dy = CurrentState.ThumbSticks.Right.Y - PastState.ThumbSticks.Right.Y;
                RightThumbstickDeltaAngle = Math.Atan2(dy, dx) * (180 / Math.PI) < 0 ? 360 + Math.Atan2(dy, dx) * (180 / Math.PI) : Math.Atan2(dy, dx) * (180 / Math.PI);
                bool dlt = dx < -0.5f;
                bool drt = dx > 0.5f;
                bool dut = dy > 0.5f;
                bool ddt = dy < -0.5f;
                RightThumbstickDeltaDirection = _n;
                RightThumbstickDeltaDirection = dlt == drt ? _n : dlt ? _l : drt ? _r : _n;
                RightThumbstickDeltaDirection = dut == ddt ? dlt == drt ? _n : dlt ? _l : drt ? _r : _n : dlt && dut ? _lu : dlt && ddt ? _ld : drt && dut ? _ru : drt && ddt ? _rd : dlt == drt ? _n : dlt ? _l : drt ? _r : _n;
                RightThumbstickDeltaDirection = RightThumbstickDeltaDirection == _n ? ut ? _u : dt ? _d : _n : RightThumbstickDeltaDirection;
                RightThumbstickDelta = new Vector2(dx, dy);
            }
            if(HasLeftTrigger)
            {
                LeftTriggerPower = Triggers.Left;
            }
            if(HasRightTrigger)
            {
                RightTriggerPower = Triggers.Right;
            }
        }

        public void StartVibrate(float left, float right)
        {
            if (!IsConnected) { return; }
            GamePad.SetVibration(AssociatedPlayer, left, right);
        }
        public void StopVibrate()
        {
            if (!IsConnected) { return; }
            GamePad.SetVibration(AssociatedPlayer, 0.0f, 0.0f);
            _vibrating = false;
        }

        public void Vibrate(int duration, float left, float right)
        {
            _vibrationStartTime = Extensions.GetMS();
            _vibrationLeft = left;
            _vibrationRight = right;
            _vibrationDuration = _vibrating ? duration : _vibrationDuration + duration;
        }

        #region Input Methods
        
        public bool LeftThumbstickDeltaChanged(Direction to)
        {
            return LeftThumbstickDeltaDirection == to ? LeftThumbstickDeltaDirection != _pastLeftThumbstickDeltaDirection : false;
        }
        public bool RightThumbstickDeltaChanged(Direction to)
        {
            return RightThumbstickDeltaDirection == to ? RightThumbstickDeltaDirection != _pastRightThumbstickDeltaDirection : false;
        }
        
        public bool LeftThumbstickDirectionChanged(Direction to)
        {
            return LeftThumbstickDirection == to ? LeftThumbstickDirection!=_pastLeftThumbstickDirection:false;
        }
        public bool RightThumbstickDirectionChanged(Direction to)
        {
            return RightThumbstickDirection == to ? RightThumbstickDirection != _pastRightThumbstickDirection : false;
        }

        public bool WasButtonPressed(Buttons button)
        {
            return CurrentState.IsButtonDown(button) && PastState.IsButtonUp(button);
        }
        public bool WasButtonReleased(Buttons button)
        {
            return PastState.IsButtonDown(button) && CurrentState.IsButtonUp(button);
        }

        public bool IsButtonUp(Buttons button)
        {
            return CurrentState.IsButtonUp(button);
        }
        public bool IsButtonDown(Buttons button)
        {
            return CurrentState.IsButtonDown(button);
        }
        
        public bool Connected()
        {
            return CurrentState.IsConnected && !PastState.IsConnected;
        }
        public bool Disconnected()
        {
            return !CurrentState.IsConnected && PastState.IsConnected;
        }

        #endregion

        public string ToString()
        {
            string ret = $"Controller of {AssociatedPlayer}:\n\n";
            ret += $"\tConnected : {IsConnected}\n";
            if(IsConnected)
            {
                ret += $"\tLeft Thumbstick:\n";
                ret += $"\t\tAngle : {LeftThumbstickAngle}\n";
                ret += $"\t\tDirection : {LeftThumbstickDirection}\n";
                ret += $"\t\tDelta:\n";
                ret += $"\t\t\tAngle : {LeftThumbstickDeltaAngle}\n";
                ret += $"\t\t\tDirection : {LeftThumbstickDeltaDirection}\n\n";
                ret += $"\tRight Thumbstick:\n";
                ret += $"\t\tAngle : {RightThumbstickAngle}\n";
                ret += $"\t\tDirection : {RightThumbstickDirection}\n";
                ret += $"\t\tDelta:\n";
                ret += $"\t\t\tAngle : {RightThumbstickDeltaAngle}\n";
                ret += $"\t\t\tDirection : {RightThumbstickDeltaDirection}\n\n";
                ret += $"\tLeft Trigger:\n";
                ret += $"\t\tPower : {LeftTriggerPower}\n\n";
                ret += $"\tRight Trigger:\n";
                ret += $"\t\tPower : {RightTriggerPower}\n";

            }
            return ret;
        }

        public void UpdateState()
        {
            PastState = CurrentState;
            CurrentState = GamePad.GetState(AssociatedPlayer);
            Capabilities = GamePad.GetCapabilities(AssociatedPlayer);
            UpdateVariables();
            if(_vibrating)
            {
                GamePad.SetVibration(AssociatedPlayer, _vibrationLeft, _vibrationRight);
                if(Extensions.GetMS()-_vibrationStartTime>_vibrationDuration)
                {
                    GamePad.SetVibration(AssociatedPlayer, 0.0f, 0.0f);
                    _vibrating = false;
                }
            }
        }
    }
}
