using Core.Dependencies;
using Entities;
using Players;
using UnityEngine;

namespace _01.Script.LKW.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        [Inject,SerializeField] private Player player;
        [SerializeField] private EntityFinderSO playerFinder;

        
        private void Awake()
        {
            playerFinder.SetTarget(player);
        }
    }
}