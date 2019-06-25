using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace RTS1
{
    public static class AudioController
    {

        private static readonly Dictionary<string, SoundEffect> SoundEffects = new Dictionary<string, SoundEffect>();
        private static readonly Dictionary<string, Song> Songs = new Dictionary<string, Song>();
        
        public static bool AddSfx(string name)
        {
            try
            {
                if (SoundEffects.ContainsKey(name))
                {
                    return false;
                }
                SoundEffects.Add(name, Program.Content.Load<SoundEffect>(name));
                return true;
            }
            catch (ContentLoadException)
            {
                return false;
            }
        }
        
        public static bool AddMusic(string name)
        {
            if (Songs.ContainsKey(name))
            {
                return false;
            }
            try
            {
                Songs.Add(name, Program.Content.Load<Song>(name));
            }
            catch (ContentLoadException)
            {
                return false;
            }
            return true;
        }
        
        public static SoundEffectInstance PlaySfx(string name)
        {
            if (SoundEffects.ContainsKey(name))
            {

                var instance = SoundEffects[name].CreateInstance();
                instance.Play();
                return instance;
            }

            try
            {
                SoundEffects.Add(name, Program.Content.Load<SoundEffect>(name));
                var instance = SoundEffects[name].CreateInstance();
                instance.Play();
                return instance;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string _curSong;

	    public static string CurrentMusic
        {
            set
            {
                _curSong = value;
                if (value != null)
                {
                    try
                    {
                        MediaPlayer.Play(Songs.ContainsKey(value) ? Songs[value] : Program.Content.Load<Song>(value));
                    }
                    catch (ContentLoadException)
                    {
                        _curSong = null;
                        MediaPlayer.Stop();
                    }
                }
                else
                {
                    MediaPlayer.Stop();
                }
            }

            get => _curSong;
        }

	    public static double MusicVolume
        {
            set => MediaPlayer.Volume = (float)value;
            get => MediaPlayer.Volume;
        }
        
        public static void Update(TimeSpan dt)
        {
            if (!_fading)
                return;
            MusicVolume -= _outVolStep * dt.Seconds;
            if (!(MusicVolume <= 0))
                return;
            _fading = false;
            CurrentMusic = _songAfterFade;
            MusicVolume = _vol;
        }
        
        public static void Fadeout(double outDuration)
        {
            FadeTo(outDuration, null);
        }
        
        public static void FadeTo(double outDuration, string newSong)
        {
            _fading = true;
            _vol = (float)MusicVolume;
            _outVolStep = (float)(_vol / outDuration);
            _songAfterFade = newSong;
        }
        private static float _vol;
        private static float _outVolStep;
        private static bool _fading;
        private static string _songAfterFade;
        
	    public static void Pause()
        {
            MediaPlayer.Pause();
        }
        
	    public static void Play()
        {
            MediaPlayer.Resume();
        }

        public static bool Looping
        {
            set => MediaPlayer.IsRepeating = value;
            get => MediaPlayer.IsRepeating;
        }
    }

}
