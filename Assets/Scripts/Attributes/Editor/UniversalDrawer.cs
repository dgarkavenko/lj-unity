using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UberDrawAttribute))]
public class UniversalDrawer : PropertyDrawer
{
    static Dictionary<string, Val[]> _valsesDict = new Dictionary<string, Val[]>();
    const int Height = 16;


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        UberDrawAttribute ueAttribute = (UberDrawAttribute)attribute;
        return ueAttribute.Lines * Height;
    }

    public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
    {
        UberDrawAttribute ueAttribute = (UberDrawAttribute)attribute;
        EditorGUI.BeginProperty(pos, label, property);
        if (ueAttribute.WithLable) pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

        Val[] vals = GetValues(ueAttribute);
        int x = (int)pos.x;
        pos.height = Height;

        foreach (Val val in vals)
        {
            if (val.NewLine)
            {
                x = (int)pos.x;
                pos.y += Height;
            }
            //EditorGUIUtility.fieldWidth = 30;


            if (val.Type == Val.ValType.Normal) 
                EditorGUI.PropertyField(GetRect(pos, ref x, val.Width), property.FindPropertyRelative(val.Name), GUIContent.none);
            else if (val.Type == Val.ValType.Slider) 
                Slider(GetRect(pos, ref x, val.Width), property.FindPropertyRelative(val.Name), val.DataF[0], val.DataF[1]);
           
        }

        EditorGUI.EndProperty();
    }

    static void Slider(Rect rect, SerializedProperty property, float min, float max)
    {

        switch (property.propertyType)
        {
            case SerializedPropertyType.Float:
            {
                EditorGUI.Slider(rect, property, min, max, GUIContent.none);
            }
                break;
            case SerializedPropertyType.Integer:
            {
                EditorGUI.IntSlider(rect, property, (int)min, (int)max, GUIContent.none);
            }
                break;
            case SerializedPropertyType.Vector2:
            {
                Vector2 value = property.vector2Value;
                EditorGUI.MinMaxSlider(rect, ref value.x, ref value.y, min, max);
                property.vector2Value = value;
            }
                break;
        }
    }
    static Rect GetRect(Rect source, ref int x, int width)
    {
        source.xMin = x;
        source.width = width;
        x += width;
        return source;
    }
    static Val[] GetValues(UberDrawAttribute ueAttribute)
    {
        string data = ueAttribute.Data;

        Val[] values;
        if (_valsesDict.TryGetValue(data, out values))
            return values;
        values = Parse(data);
        _valsesDict.Add(data, values);
        return values;
    }
    static Val[] Parse(string data)
    {
        LinkedList<Val> valList = new LinkedList<Val>();
        string[] lines = data.Split('\n');
        bool newLine = false;
        foreach (string l in lines)
        {
            string[] columns = l.Split(';');
            foreach (string c in columns)
            {
                string[] vals = c.Split(' ');
                if (vals.Length == 2) valList.AddLast(new Val(vals[0], int.Parse(vals[1]), newLine));
                else  valList.AddLast(new Val(vals[0], int.Parse(vals[1]), vals, newLine));
                newLine = false;
            }
            newLine = true;
        }
        Val[] valArray = new Val[valList.Count];
        valList.CopyTo(valArray, 0);
        return valArray;
    }

    class Val
    {
        public string Name;
        public int Width;
        public bool NewLine;
        public ValType Type;
        public float[] DataF;
        
        public Val(string name, int width, string[] vals, bool newLine = false)
        {
            Name = name;
            Width = width;
            NewLine = newLine;

            string type = vals[2];
            switch (type)
            {
                case "S":
                case "s":
                    Type = ValType.Slider;
                    DataF = vals.Length == 5 ? new[] { float.Parse(vals[3]), float.Parse(vals[4]) } : new[] { 0f, 1f };
                    break;

                default:
                    Type = ValType.Normal;
                    break;
            }
        }
        public Val(string name, int width, bool newLine = false)
        {
            Name = name;
            Width = width;
            NewLine = newLine;
            Type = ValType.Normal;
        }
        
        public enum ValType
        {
            Normal,
            Slider
        }
    }
}