using UI.Setting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Title
{
    public class TitleView : MonoBehaviour
    {
        [SerializeField] private GameObject characterUI;
        [SerializeField] private string gameSceneName;

        [Header("<color=yellow>[ GameOver UI ]</color>")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
            startButton.onClick.AddListener(HandleStartButton);
            settingButton.onClick.AddListener(HandleSettingButton);
            quitButton.onClick.AddListener(HandleQuitButton);
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(HandleStartButton);
            settingButton.onClick.RemoveListener(HandleSettingButton);
            quitButton.onClick.RemoveListener(HandleQuitButton);
        }

        #region Event Method

        private void HandleStartButton()
        {
            characterUI.SetActive(true);
            SettingViewModel.Instance.Close();
        }

        private void HandleSettingButton()
        {
            characterUI.SetActive(false);
            SettingViewModel.Instance.Open();
        }
        
        private void HandleQuitButton() => Application.Quit();

        #endregion
    }
}