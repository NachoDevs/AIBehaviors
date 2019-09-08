using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public enum IngredientUnit { Spoon, Cup, Bowl, Piece }

[Serializable]
public class SM_State
{
    public int num1 = 1;
    public IngredientUnit ingredient;
}

[CustomPropertyDrawer(typeof(SM_State))]
public class StateDrawerUIE : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();

        // Create property fields.
        var num1Field = new PropertyField(property.FindPropertyRelative("num1"));
        var ingredientField = new PropertyField(property.FindPropertyRelative("ingredient"));


        // Add fields to the container.
        container.Add(num1Field);
        container.Add(ingredientField);

        return container;
    }
}