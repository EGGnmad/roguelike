using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MVVM
{
    [CustomPropertyDrawer(typeof(PropertySelector))]
    public class PropertySelectorDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // References
            SerializedProperty objProperty = property.FindPropertyRelative("obj");
            Component comp = objProperty.objectReferenceValue as Component;

            // Create property container element.
            var container = new VisualElement();

            // Object Field
            var objField = new PropertyField(objProperty, "Object");
            container.Add(objField);

            // Properties Field
            if (comp != null)
            {
                var properties = comp.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                List<string> propertiesString = properties.Select(x => x.Name).ToList();
                var dropdownField = new DropdownField("Property", propertiesString, 0);

                // DropdownField value changed event
                dropdownField.RegisterValueChangedCallback(evt =>
                {
                    string selectedProperty = evt.newValue;
                    PropertyInfo propertyInfo = comp.GetType().GetProperty(selectedProperty, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo != null)
                    {
                        // Use reflection to set the private field value
                        SerializedProperty propertyInfoField = property.FindPropertyRelative("_property");
                        Debug.Log(propertyInfoField == null);

                        if (propertyInfoField != null)
                        {
                            // Set the PropertyInfo field using reflection
                            propertyInfoField.managedReferenceValue = propertyInfo;
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    }
                });
                container.Add(dropdownField);
            }

            return container;
        }
    }
}