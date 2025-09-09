using System;
using Core;
using Core.Define;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseViewModel
    {
        private static PauseViewModel _instance;
        public static PauseViewModel Instance => _instance ??= new PauseViewModel();

        private readonly PauseModel _model = new();

        public ObservableProperty<bool> IsOpen { get; } = new();
        public ObservableProperty<int> ScreenModeIndex { get; } = new();
        public ObservableProperty<float> MasterVolume { get; } = new(ConstDefine.DefaultVolume);
        public ObservableProperty<float> BGMVolume { get; } = new(ConstDefine.DefaultVolume);
        public ObservableProperty<float> SFXVolume { get; } = new(ConstDefine.DefaultVolume);

        private PauseViewModel()
        {
        } // private 생성자 (다른 곳에서 생성 못하게)

        public void Toggle() => IsOpen.Value = !IsOpen.Value;



        public void ApplyScreenMode(int index)
        {
            try
            {
                _model.ApplyScreenMode(index);
                ScreenModeIndex.Value = index;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                UnityLogger.LogError(exception.Message);
            }
        }

        public void SetMasterVolume(float volume)
        {
            try
            {
                _model.SetMasterVolume(volume);
                MasterVolume.Value = volume;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                UnityLogger.LogError(exception.Message);
            }
        }

        public void SetMusicVolume(float volume)
        {
            try
            {
                _model.SetMusicVolume(volume);
                BGMVolume.Value = volume;
            }
            catch (ArgumentException exception)
            {
                UnityLogger.LogError(exception.Message);
            }
        }

        public void SetEffectVolume(float volume)
        {
            try
            {
                _model.SetEffectVolume(volume);
                SFXVolume.Value = volume;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                UnityLogger.LogError(exception.Message);
            }
        }
    }
}