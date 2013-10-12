using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Asg2_6262732
{
    public class Audio
    {
        #region Fields
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        SoundBank soundEffect;
        Cue _Background = null;
        public Cue _NextLevel
        {
            get;
            private set;
        }
        Cue _GameOver = null;
        public bool _EndPlay
        {
            get;
            private set;
        }
        public bool _NextLevelSound
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public void Initialize()
        {
            audioEngine = new AudioEngine(@"Content\Audio\FoodFightNow.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Long.xsb");
            soundEffect = new SoundBank(audioEngine, @"Content\Audio\Short.xsb");
            _GameOver = soundBank.GetCue("GameOver");
            _Background = soundBank.GetCue("Background");
            _Background.Play();
            _EndPlay = false;
            _NextLevel = soundBank.GetCue("NextLevel");
            _Background.Play();
            _NextLevelSound = false;
        }
        public void StartNextLevel()
        {
            if (!_NextLevelSound)
            {
                _NextLevel.Play();
                _NextLevelSound = true;
            }
        }
        public void StartGameOver()
        {
            if (!_EndPlay)
            {
                _Background.Stop(AudioStopOptions.Immediate);
                _GameOver.Play();
                _EndPlay = true;
            }

        }
        public void StartGame()
        {
            _EndPlay = false;
            _GameOver.Stop(AudioStopOptions.Immediate);
        }
        public void ResetSounds()
        {
            _NextLevelSound = false;
            _EndPlay = false;
        }
        public void Update()
        {
            if (!_NextLevelSound && !_EndPlay && !_Background.IsPlaying)
            {
                _Background.Play();
            }
        }
        public void Play(String str)
        {
            soundEffect.GetCue(str).Play();
        }
        #endregion
    }
}
