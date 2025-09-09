using System.Collections.Generic;
using Core;
using Core.Define;
using Core.Dependencies;
using Core.GameEvent;
using Players;
using Score.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame
{
    public class InGameView : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO playerChannel;

        [Header("<color=yellow>[ InGame UI ]</color>")]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Image gunImage;
        [SerializeField] private GameObject bulletLayoutGroup, skillLayoutGroup;
        [SerializeField] private GameObject skillGaugeBar;
        
        private readonly List<BulletUI> _bulletUIList = new();
        private readonly List<SkillGaugeBarUI> _gaugeBarList = new();
        
        [Inject] private Player _player;
        private int _currentAmmo;

        private void Start()
        {
            SetGunUI(_player.CharacterData.gunData.gunSprite);
            SetBulletUI();
            SetGaugeBar();

            playerChannel.AddListener<PlayerBulletShootEvent>(FireBulletUI);
            playerChannel.AddListener<PlayerReloadEvent>(ReloadBulletUI);
            playerChannel.AddListener<PlayerSkillGaugeEvent>(SetGaugeValue);
            
            playerChannel.RaiseEvent(PlayerEvents.playerSkillGaugeEvent.Initialize(0));
        }

        private void OnDestroy()
        {
            UnbindAll();
        }

        private void Update()
        {
            timerText.text = ScoreManager.Instance.FormatSurviveTime();
        }

        public void DisabledUI()
        {
            bulletLayoutGroup.SetActive(false);
            skillLayoutGroup.SetActive(false);
            timerText.gameObject.SetActive(false);
            gunImage.gameObject.SetActive(false);
        }

        private void UnbindAll()
        {
            playerChannel.RemoveListener<PlayerBulletShootEvent>(FireBulletUI);
            playerChannel.RemoveListener<PlayerReloadEvent>(ReloadBulletUI);
            playerChannel.RemoveListener<PlayerSkillGaugeEvent>(SetGaugeValue);
        }

        private void SetGaugeBar()
        {
            var fillColor = _player.CharacterData.showDownData.energyColor;
            var gaugeWidth = ConstDefine.GaugeBarWidth / (float)_player.CharacterData.showDownData.maxSkillEnergy;

            for (int i = 0; i < _player.CharacterData.showDownData.maxSkillEnergy; ++i)
            {
                var gaugeBar = Instantiate(skillGaugeBar, skillLayoutGroup.transform,
                    true).GetComponent<SkillGaugeBarUI>();

                var gaugeRect = gaugeBar.GetComponent<RectTransform>();
                gaugeRect.sizeDelta = new Vector2(gaugeWidth, gaugeRect.sizeDelta.y);

                gaugeBar.SetFillColor(fillColor);
                gaugeBar.SetGaugeWidth(gaugeWidth);
                _gaugeBarList.Add(gaugeBar);
            }
        }

        private void SetGunUI(Sprite gunSprite)
        {
            gunImage.sprite = gunSprite;
            gunImage.SetNativeSize();
        }

        private void SetBulletUI()
        {
            for (int i = 0; i < _player.CharacterData.gunData.maxAmmo; ++i)
            {
                var bulletUI = Instantiate(_player.CharacterData.gunData.bulletUI, bulletLayoutGroup.transform,
                    true).GetComponent<BulletUI>();

                _bulletUIList.Add(bulletUI);
            }

            _currentAmmo = _bulletUIList.Count;
        }

        private void ReloadBulletUI(PlayerReloadEvent playerReloadEvent)
        {
            while (_currentAmmo < playerReloadEvent.currentAmmoCount)
                _bulletUIList[_currentAmmo++].Reload();

            _currentAmmo = playerReloadEvent.currentAmmoCount;
        }

        private void FireBulletUI(PlayerBulletShootEvent playerBulletShootEvent)
        {
            _bulletUIList[--_currentAmmo].Fire();
        }

        private void SetGaugeValue(PlayerSkillGaugeEvent playerSkillGaugeEvent)
        {
            var fullGaugeColor = _player.CharacterData.showDownData.fullEnergyColor;
            var gaugeColor = _player.CharacterData.showDownData.energyColor;
            var currentGauge = playerSkillGaugeEvent.gauge;
            UnityLogger.Log($"Gauge : {currentGauge}");
            
            var fullGauge = Mathf.FloorToInt(currentGauge);
            var remainingGauge = currentGauge % 1f;

            for (int i = 0; i < _gaugeBarList.Count; ++i)
                if (i < fullGauge)
                {
                    _gaugeBarList[i].SetFillColor(fullGaugeColor);
                    _gaugeBarList[i].SetGaugeValue(1f);
                }
                else if (i == fullGauge && remainingGauge > 0)
                {
                    _gaugeBarList[i].SetFillColor(gaugeColor);
                    _gaugeBarList[i].SetGaugeValue(remainingGauge);
                }
                else
                {
                    _gaugeBarList[i].SetFillColor(gaugeColor);
                    _gaugeBarList[i].SetGaugeValue(0f);
                }
        }
    }
}