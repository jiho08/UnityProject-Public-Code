using System.Collections.Generic;
using UnityEngine;

namespace Entities.Stat
{
    [CreateAssetMenu(fileName = "StatSO", menuName = "SO/StatSystem/StatSO", order = 0)]
    public class StatSO : ScriptableObject
    {
        public delegate void ValueChangeHandler(StatSO stat, float current, float previous);

        public event ValueChangeHandler OnValueChange;

        public string statName;
        [TextArea] public string description;

        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;
        [SerializeField] private float baseValue, minValue, maxValue;

        private Dictionary<object, float> _modifyDictionary = new Dictionary<object, float>();

        [field: SerializeField] public bool IsPercent { get; private set; }

        private float _modifiedValue = 0;

        #region Property section

        public Sprite Icon => icon;

        public float MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }

        public float MinValue
        {
            get => minValue;
            set => minValue = value;
        }

        public float Value => Mathf.Clamp(baseValue + _modifiedValue, minValue, maxValue);
        public bool IsMax => Mathf.Approximately(Value, maxValue);
        public bool IsMin => Mathf.Approximately(Value, minValue);

        public float BaseValue
        {
            get => baseValue;
            set
            {
                float previous = Value;
                baseValue = Mathf.Clamp(value, MinValue, MaxValue);
                TryInvokeValueChangedEvent(value, previous);
            }
        }

        #endregion

        public void AddModifier(object key, float value)
        {
            if (_modifyDictionary.ContainsKey(key)) return;
            var prevValue = Value;

            _modifiedValue += value;
            _modifyDictionary.Add(key, value);

            TryInvokeValueChangedEvent(Value, prevValue);
        }

        public void RemoveModifier(object key)
        {
            if (_modifyDictionary.TryGetValue(key, out var value))
            {
                var prevValue = Value;
                _modifiedValue -= value;
                _modifyDictionary.Remove(key);

                TryInvokeValueChangedEvent(Value, prevValue);
            }
        }

        public void ClearAllModifiers()
        {
            var prevValue = Value;
            _modifyDictionary.Clear();
            _modifiedValue = 0;
            TryInvokeValueChangedEvent(Value, prevValue);
        }

        private void TryInvokeValueChangedEvent(float current, float previous)
        {
            if (Mathf.Approximately(current, previous)) return;
            OnValueChange?.Invoke(this, current, previous);
        }


        public object Clone() => Instantiate(this);
    }
}