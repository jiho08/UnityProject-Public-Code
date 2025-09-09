using UnityEngine;
using DG.Tweening;

namespace UI.InGame
{
    public class BulletUI : MonoBehaviour
    {
        private bool isFired;
        
        public bool IsFired => isFired;
        
        [ContextMenu("발사")]
        public void Fire()
        {
            isFired = true;
            OnFireChanged();
        }
        
        [ContextMenu("장전")]
        public void Reload()
        {
            isFired = false;
            OnFireChanged();
        }

        private void OnFireChanged()
        {
            if (isFired)
                transform.DOLocalMove(new Vector3(transform.localPosition.x, 75, 0),
                    1).SetEase(Ease.OutElastic);
            else
                transform.DOLocalMove(new Vector3(transform.localPosition.x, -50, 0),
                    1).SetEase(Ease.OutElastic);
        }
    }
}