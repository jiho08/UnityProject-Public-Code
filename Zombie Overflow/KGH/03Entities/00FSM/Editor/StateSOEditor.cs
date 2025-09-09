using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Animations;

namespace FSM.Editor
{
    [CustomEditor(typeof(StateSO))]
    public class StateSOEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset editorUI = default;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            editorUI.CloneTree(root);

            var dropdownField = root.Q<DropdownField>("ClassDropDownField");
            CreateDropdownList(dropdownField);

            return root;
        }

        private void CreateDropdownList(DropdownField dropdown)
        {
            dropdown.choices.Clear();

            var assembly = Assembly.GetAssembly(typeof(StateSO));
            var derivedTypes = assembly.GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EntityState))).Select(type => type.FullName)
                .ToList();
            
            dropdown.choices.AddRange(derivedTypes);
        }
    }
}