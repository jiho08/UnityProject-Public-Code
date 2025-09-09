using System;
using UnityEngine;

namespace _01.Script.KGH._01Player
{
    public class PlayerMuzzle : MonoBehaviour
    {
        [SerializeField] private Transform[] muzzles;
        private void Start()
        {
            foreach (var muzzle in muzzles)
            {
                if (muzzle.gameObject.activeSelf)
                {
                    transform.position = muzzle.position;
                    transform.rotation = muzzle.rotation;
                }
            }
        }
    }
}