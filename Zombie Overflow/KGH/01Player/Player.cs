using System.Collections;
using System.Collections.Generic;
using Core;
using Entities;
using Input.InputScript;
using Animations;
using Combat;
using Combat.Skills;
using Core.Define;
using Core.Dependencies;
using Core.GameEvent;
using Entities.Stat;
using FSM;
using Score.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Players
{
    public class Player : Entity, IDependencyProvider, IHittable
    {
        [field: SerializeField] public CharacterSO CharacterData { get; set; }
        [field: SerializeField] public PlayerInputSO PlayerInputSO { get; private set; }

        [field: Header("States")] [SerializeField]
        private StateSO[] topStates;

        [SerializeField] private StateSO[] bottomStates;

        [field: Header("Animation Parameters")]
        [field: SerializeField]
        public AnimParamSO DodgeSpeedParam { get; private set; }

        [field: SerializeField] public AnimParamSO ReloadSpeedParam { get; private set; }

        [field: Header("Stats")]
        [field: SerializeField]
        public StatSO DodgeSpeedStat { get; private set; }

        [field: SerializeField] public StatSO ReloadSpeedStat { get; private set; }

        [field: SerializeField] public StatSO MoveSpeedStat { get; set; }

        [field: Header("Event Channels")]
        [field: SerializeField]
        public GameEventChannelSO PlayerEventChannel { get; private set; }

        [field: Header("ECT")] public LayerMask DodgeLayerMask { get; private set; }
        private Dictionary<EnumDefine.StateType, EntityStateMachine> _stateMachines;


        [Provide]
        public Player GetPlayer() => this;

        public bool DoesLookAtMouse { get; set; } = true;

        private SkillComponent _skillComponent;


        protected override void Awake()
        {
            base.Awake();
            DodgeLayerMask = LayerMask.NameToLayer("Dodge");
            _stateMachines = new Dictionary<EnumDefine.StateType, EntityStateMachine>
            {
                { EnumDefine.StateType.Top, new EntityStateMachine(this, topStates) },
                { EnumDefine.StateType.Bottom, new EntityStateMachine(this, bottomStates) }
            };
            PlayerInputSO.SetPlaneHeight(GetCompo<RigController>().transform.position.y);
            CharacterData = GameManager.Instance.CharacterSO;
            SetUpPlayer();

            _skillComponent = GetCompo<SkillComponent>(true);
        }

        private void SetUpPlayer()
        {
            var playerCompos = GetComponentsInChildren<IPlayerComponent>();
            foreach (var compo in playerCompos)
            {
                compo.SetUpPlayer(CharacterData);
            }
        }

        private void Start()
        {
            foreach (var stateMachine in _stateMachines.Values)
            {
                stateMachine.ChangeState("IDLE");
            }

            PlayerEventChannel.AddListener<PlayerSkillGaugeEvent>(HandleSkillGauge);
            PlayerEventChannel.AddListener<PlayerInputToggleEvent>(HandleToggleEvent);
        }

        private void OnDestroy()
        {
            foreach (var stateMachines in _stateMachines.Values)
            {
                stateMachines.DestroyStateMachine();
            }

            PlayerEventChannel.RemoveListener<PlayerSkillGaugeEvent>(HandleSkillGauge);
            PlayerEventChannel.RemoveListener<PlayerInputToggleEvent>(HandleToggleEvent);
        }

        private void HandleToggleEvent(PlayerInputToggleEvent obj) => PlayerInputSO.ToggleInput(obj.isEnabled);

        private void HandleSkillGauge(PlayerSkillGaugeEvent evt)
        {
            evt.gauge = _skillComponent.SetSkillGauge(evt.gauge);
        }

        protected override void HandleHit()
        {
            foreach (var stateMachine in _stateMachines.Values)
            {
                stateMachine.ChangeState("DEAD");
            }
        }

        private void Update()
        {
            foreach (var stateMachine in _stateMachines.Values)
            {
                stateMachine.UpdateStateMachine();
            }
        }

        public void ChangeState(string stateName, EnumDefine.StateType stateType)
        {
            if (_stateMachines.TryGetValue(stateType, out var stateMachine))
            {
                stateMachine.ChangeState(stateName);
            }
            else
            {
                UnityLogger.LogError($"State machine of type {stateType} not found.");
            }
        }

        public void TakeHit(Vector3 direction)
        {
            Debug.Log("TakeHit");
            OnHit?.Invoke();
        }

        public Coroutine CoroutineHelper(IEnumerator enumerator)
        {
            return StartCoroutine(enumerator);
        }
    }
}