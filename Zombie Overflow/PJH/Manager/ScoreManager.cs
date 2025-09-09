using Core;
using Core.Define;
using Core.GameEvent;
using UnityEngine;

namespace Score.Manager
{
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        [SerializeField] private GameEventChannelSO enemyChannel;
        
        private float _surviveTime;
        private float _bestSurviveTime;
        
        private int _killCount;
        private int _bestKillCount;

        private bool _isPlaying;
        
        public float SurviveTime => _surviveTime;
        
        public float BestSurviveTime => _bestSurviveTime;
        
        public int KillCount => _killCount;
        
        public int BestKillCount => _bestKillCount;

        private void Start()
        {
            enemyChannel.AddListener<EnemyDeadEvent>(AddKillCount);
            
            _bestSurviveTime = PlayerPrefs.GetFloat(ConstDefine.BestSurviveTimePrefsKey, 0);
            _bestKillCount = PlayerPrefs.GetInt(ConstDefine.BestKillCountPrefsKey, 0);
            
            StartTimer();
        }

        private void OnDestroy()
        {
            enemyChannel.RemoveListener<EnemyDeadEvent>(AddKillCount);
        }

        private void Update()
        {
            AddTimer();
        }

        private void AddTimer()
        {
            if (!_isPlaying)
                return;
            
            _surviveTime += Time.deltaTime;
        }
        
        private void AddKillCount(EnemyDeadEvent deadEvent)
        {
            ++_killCount;
        }
        
        public void ResetScore()
        {
            _isPlaying = false;
            _surviveTime = 0;
            _killCount = 0;
        }

        public void StartTimer()
        {
            _isPlaying = true;
        }

        public void SaveScore()
        {
            if (_surviveTime > _bestSurviveTime)
            {
                _bestSurviveTime = _surviveTime;
                PlayerPrefs.SetFloat(ConstDefine.BestSurviveTimePrefsKey, _bestSurviveTime);
            }
            
            if (_killCount > _bestKillCount)
            {
                _bestKillCount = _killCount;
                PlayerPrefs.SetInt(ConstDefine.BestKillCountPrefsKey, _bestKillCount);
            }
        }
        
        public string FormatTime(float time)
        {
            var minutes = Mathf.FloorToInt(time / 60);
            var seconds = Mathf.FloorToInt(time % 60);
            
            return $"{minutes:00} : {seconds:00}";
        }

        public string FormatSurviveTime() => FormatTime(_surviveTime);
        
        public string FormatBestSurviveTime() => FormatTime(_bestSurviveTime);
    }
}