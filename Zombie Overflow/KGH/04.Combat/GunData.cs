using Feedbacks.SFX;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "GunData", menuName = "SO/Combat/GunData", order = 0)] //바꾸고 싶음 바꾸세용
    public class GunData : ScriptableObject
    {
        public Sprite gunSprite;
        public GameObject bulletUI;
        [Header("GunSetting")]
        public int maxAmmo;
        public float bulletSpeedMultiplier = 1;
        public bool multiBullet;
        public int bulletCount = 1;
        public float bulletSpreadAngle = 0;
        [Header("Effects")]
        public SoundDataSO shootSound;
        public SoundDataSO reloadSound;
        public SoundDataSO emptySound;
    }
}