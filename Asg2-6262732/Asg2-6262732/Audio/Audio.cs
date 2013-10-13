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
//        SoundBank soundEffect;
        Cue _Background = null;
        public Cue _Victory
        {
            get;
            private set;
        }
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
        public bool _VictorySound
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
            soundBank = new SoundBank(audioEngine, @"Content\Audio\GameMusic.xsb");
            
            _GameOver = soundBank.GetCue("GameOver");
            _Background = soundBank.GetCue("Background");
            _Background.Play();
            _EndPlay = false;
            _NextLevel = soundBank.GetCue("NextLevel");
            _NextLevelSound = false;
            _Victory = soundBank.GetCue("Victory");
            _VictorySound = false;
        }
        public void StartVictory()
        {
            if (!_VictorySound)
            {
                _Background.Stop(AudioStopOptions.Immediate);
                _Victory.Play();
                _VictorySound = true;
            }
        }
        public void StartNextLevel()
        {
            if (!_NextLevelSound)
            {
                _Background.Stop(AudioStopOptions.Immediate);
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
            _VictorySound = false;
        }
        public void Update()
        {
            if (!_VictorySound && !_NextLevelSound && !_EndPlay && !_Background.IsPlaying)
            {
                _Background.Play();
            }
        }
        public void Play(String bank, String effect)
        {
            new SoundBank(audioEngine, @"Content\Audio\" + bank +".xsb").GetCue(effect).Play();
//            soundEffect.GetCue(str).Play();
        }
        #endregion
    }
}
