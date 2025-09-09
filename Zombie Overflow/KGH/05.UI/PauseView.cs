using System.Collections;
using Core.Define;
using Input.InputScript;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PauseView : MonoBehaviour, IView
    {
        [SerializeField] private UIInputSO uiInput;
        [SerializeField] private PlayerInputSO playerInput;

        [Header("<color=yellow>[ Setting UI ]</color>")] [SerializeField]
        private GameObject settingUI;

        [SerializeField] private TMP_Dropdown screenModeDropdown;
        [SerializeField] private Button resumeButton, titleButton;
        [SerializeField] private Slider masterVolumeSlider, effectVolumeSlider, musicVolumeSlider;

        private PauseViewModel _viewModel;

        private bool _canPause = true;

        private void Awake()
        {
            _viewModel = PauseViewModel.Instance;

            BindViewToViewModel();
            BindViewModelToView();

            SetDefaultSetting();

            uiInput.OnEscapePressed += HandleEscapePressed;
            _viewModel.IsOpen.Value = false;
            playerInput.ToggleInput(true);
        }

        private void OnDestroy()
        {
            UnbindAll();
            uiInput.OnEscapePressed -= HandleEscapePressed;
        }

        private void HandleEscapePressed()
        {
            if (_viewModel.IsOpen.Value)
                Close();
            else
                Open();
        }

        public void BindViewToViewModel()
        {
            screenModeDropdown.onValueChanged.AddListener(_viewModel.ApplyScreenMode);
            masterVolumeSlider.onValueChanged.AddListener(_viewModel.SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(_viewModel.SetMusicVolume);
            effectVolumeSlider.onValueChanged.AddListener(_viewModel.SetEffectVolume);
            resumeButton.onClick.AddListener(Close);
            titleButton.onClick.AddListener(Title);
        }

        public void BindViewModelToView()
        {
            _viewModel.IsOpen.Subscribe(SetActive);
            _viewModel.ScreenModeIndex.Subscribe(SetScreenMode);
            _viewModel.MasterVolume.Subscribe(SetMasterVolume);
            _viewModel.BGMVolume.Subscribe(SetMusicVolume);
            _viewModel.SFXVolume.Subscribe(SetEffectVolume);
        }

        public void UnbindAll()
        {
            screenModeDropdown.onValueChanged.RemoveListener(_viewModel.ApplyScreenMode);
            masterVolumeSlider.onValueChanged.RemoveListener(_viewModel.SetMasterVolume);
            musicVolumeSlider.onValueChanged.RemoveListener(_viewModel.SetMusicVolume);
            effectVolumeSlider.onValueChanged.RemoveListener(_viewModel.SetEffectVolume);
            resumeButton.onClick.RemoveListener(Close);
            titleButton.onClick.RemoveListener(Title);

            _viewModel.IsOpen.Unsubscribe(SetActive);
            _viewModel.ScreenModeIndex.Unsubscribe(SetScreenMode);
            _viewModel.MasterVolume.Unsubscribe(SetMasterVolume);
            _viewModel.BGMVolume.Unsubscribe(SetMusicVolume);
            _viewModel.SFXVolume.Unsubscribe(SetEffectVolume);
        }

        private void SetDefaultSetting()
        {
            _viewModel.ApplyScreenMode(PlayerPrefs.GetInt(ConstDefine.ScreenModePrefsKey));
            _viewModel.SetMasterVolume(PlayerPrefs.GetFloat(ConstDefine.MasterVolumePrefsKey));
            _viewModel.SetMusicVolume(PlayerPrefs.GetFloat(ConstDefine.MusicVolumePrefsKey));
            _viewModel.SetEffectVolume(PlayerPrefs.GetFloat(ConstDefine.EffectVolumePrefsKey));
        }

        public void Open()
        {
            if (!_canPause) return;
            _viewModel.IsOpen.Value = true;
            playerInput.ToggleInput(false);
            Time.timeScale = 0;
        }

        public void Close()
        {
            _viewModel.IsOpen.Value = false;
            Time.timeScale = 1;
            StartCoroutine(WaitAndToggleInput(true, 0.1f));
        }

        public void Title()
        {
            Time.timeScale = 1;
            StartCoroutine(WaitAndToggleInput(true, 0.1f));
            SceneManager.LoadScene(0);
        }

        public void SetCanPaused(bool isPaused) => _canPause = isPaused;

        private IEnumerator WaitAndToggleInput(bool isEnabled, float waitTime)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            playerInput.ToggleInput(isEnabled);
        }

        #region Bind Method

        private void SetActive(bool isOpen) => settingUI?.SetActive(isOpen);

        private void SetScreenMode(int index) => screenModeDropdown.SetValueWithoutNotify(index);

        private void SetMasterVolume(float volume) => masterVolumeSlider.SetValueWithoutNotify(volume);

        private void SetMusicVolume(float volume) => musicVolumeSlider.SetValueWithoutNotify(volume);

        private void SetEffectVolume(float volume) => effectVolumeSlider.SetValueWithoutNotify(volume);

        #endregion
    }
}