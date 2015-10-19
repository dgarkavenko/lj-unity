using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MinMaxVec2Attribute))]
public class MinMaxVec2Drawer : PropertyDrawer
{
    static GUIStyle _fieldStyle;
    public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
    {
        const int fieldW = 60;
        const int sliderSubstrW = fieldW * 2;
        if (_fieldStyle == null)_fieldStyle = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).label;
        
        MinMaxVec2Attribute minMaxVec2 = (MinMaxVec2Attribute)attribute;

        EditorGUI.BeginProperty(pos, label, property);
        pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);



        int x = (int)pos.x;
        float xVal = property.FindPropertyRelative("x").floatValue;
        float yVal = property.FindPropertyRelative("y").floatValue;
        EditorGUI.MinMaxSlider(GetRect(pos, ref x, (int)pos.width - sliderSubstrW), ref xVal, ref yVal, minMaxVec2.Min, minMaxVec2.Max);

        GUI.color = Color.black;

        xVal = Mathf.Clamp(xVal, minMaxVec2.Min, minMaxVec2.Max);
        yVal = Mathf.Clamp(yVal, minMaxVec2.Min, minMaxVec2.Max);

        if (minMaxVec2.UseRound)
        {
            xVal = Mathf.Round(xVal * minMaxVec2.Round) * minMaxVec2.InvRound;
            yVal = Mathf.Round(yVal * minMaxVec2.Round) * minMaxVec2.InvRound;
        }

        xVal = EditorGUI.FloatField(GetRect(pos, ref x, fieldW), xVal, _fieldStyle);
        yVal = EditorGUI.FloatField(GetRect(pos, ref x, fieldW), yVal, _fieldStyle);

        property.FindPropertyRelative("x").floatValue = xVal;
        property.FindPropertyRelative("y").floatValue = yVal;

        EditorGUI.EndProperty();
    }


    static Rect GetRect(Rect source, ref int x, int width)
    {
        source.xMin = x;
        source.width = width;
        x += width;
        return source;
    }

}