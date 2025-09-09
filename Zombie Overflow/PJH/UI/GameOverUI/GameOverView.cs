using Core.GameEvent;
using Score.Manager;
using TMPro;
using UI.InGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.GameOver
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO enemyChannel;
        [SerializeField] private InGameView inGameUI;
        [SerializeField] private GameObject gameOverUI;
        
        [Header("<color=yellow>[ Game Over UI ]</color>")]
        [SerializeField] private TextMeshProUGUI surviveTimeText;
        [SerializeField] private TextMeshProUGUI bestSurviveText;
        [SerializeField] private TextMeshProUGUI killCountText;
        [SerializeField] private TextMeshProUGUI bestKillText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;
        
        private void Awake()
        {
            restartButton.onClick.AddListener(OnRestartPressed);
            quitButton.onClick.AddListener(OnQuitPressed);
        }

        private void OnDestroy()
        {
            UnbindAll();
        }

        private void UnbindAll()
        {
            restartButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }

        public void SetActive()
        {
            gameOverUI.SetActive(true);
            ScoreManager.Instance.SaveScore();
            SetScoreText();
            ScoreManager.Instance.ResetScore();
            inGameUI.DisabledUI();
        }

        private void SetScoreText()
        {
            surviveTimeText.text = ScoreManager.Instance.FormatSurviveTime();
            bestSurviveText.text = ScoreManager.Instance.FormatBestSurviveTime();
            
            killCountText.text = ScoreManager.Instance.KillCount.ToString();
            bestKillText.text = ScoreManager.Instance.BestKillCount.ToString();
        }

        private void OnRestartPressed()
        {
            ScoreManager.Instance.StartTimer();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnQuitPressed()
        {
            SceneManager.LoadScene(0);
        }
    }
}