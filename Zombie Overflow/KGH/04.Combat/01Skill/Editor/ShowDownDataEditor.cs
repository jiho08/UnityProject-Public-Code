using System.Linq;
using System.Reflection;
using Combat.Skills.ShowDown;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Combat.Skills.Editor
{
    [CustomEditor(typeof(ShowDownData))]
    public class ShowDownDataEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset customTreeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            customTreeAsset.CloneTree(root);

            var dropdownField = root.Q<DropdownField>("ClassDropDownField");
            CreateDropdownList(dropdownField);

            return root;
        }

        private void CreateDropdownList(DropdownField dropdown)
        {
            dropdown.choices.Clear();

            var assembly = Assembly.GetAssembly(typeof(ShowDownSkill));
            var derivedTypes = assembly.GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ShowDownSkill))).Select(type => type.FullName)
                .ToList();
            
            dropdown.choices.AddRange(derivedTypes);
        }
    }
}