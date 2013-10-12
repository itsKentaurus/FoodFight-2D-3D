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
        Cue _GameOver = null;
        bool _EndPlay;
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
        public void Update()
        {
            if (!_EndPlay && !_Background.IsPlaying)
                _Background.Play();
        }
        public void Play(String str)
        {
            soundEffect.GetCue(str).Play();
        }
        #endregion
    }
}
