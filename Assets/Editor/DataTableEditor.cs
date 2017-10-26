using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Culdcept {
    [CustomEditor(typeof(DataTable), true)]
    public class DataTableEditor : Editor {

        public override void OnInspectorGUI() {
            if (target is CreatureDataTable) {
                DrawTable<CreatureDataTable, CreatureData>(target);
                return;
            }
        }

        private static void DrawTable<Table, Row>(UnityEngine.Object target) {
            Table table = (Table)(object)target;
            Type tableType = typeof(Table);
            Type rowType = typeof(Row);
            FieldInfo[] fields = rowType.GetFields();
            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true));
            DrawHead(fields);
            Row[] rows = (Row[])tableType.GetField("rows").GetValue(table);
            for (int i = 0, length = rows.Length; i < length; i++) {
                object row = rows[i]; // boxing
                DrawRow(fields, row);
                rows[i] = (Row)row; // unboxing
            }
            tableType.GetField("rows").SetValue(table, rows);
            EditorGUILayout.EndVertical();
            UnityEditor.EditorUtility.SetDirty(target);
        }

        private static void DrawHead(FieldInfo[] fields) {
            EditorGUILayout.BeginHorizontal(GUI.skin.box, GUILayout.ExpandWidth(true));
            for (int i = 0, length = fields.Length; i < length; i++) {
                EditorGUILayout.LabelField(fields[i].Name, GUILayout.Width(200f));
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawRow(FieldInfo[] fields, object row) {
            // 参数 fields 本来可以从 row 里直接拿到，从 row 里拿到的 fields 跟表头其实也对应，但是逻辑上还是应该作为参数传进来。 
            EditorGUILayout.BeginHorizontal(GUI.skin.box, GUILayout.ExpandWidth(true));
            for (int j = 0, fieldsLength = fields.Length; j < fieldsLength; j++) {
                FieldInfo field = fields[j];
                if (field.Name == "id") {
                    EditorGUILayout.LabelField(field.GetValue(row).ToString(), GUILayout.Width(200f));
                    continue;
                }
                Type fieldType = field.FieldType;
                if (fieldType == typeof(bool)) {
                    field.SetValue(row, EditorGUILayout.Toggle((bool)field.GetValue(row), GUILayout.Width(200f)));
                    continue;
                }
                if (fieldType == typeof(int)) {
                    field.SetValue(row, EditorGUILayout.IntField((int)field.GetValue(row), GUILayout.Width(200f)));
                    continue;
                }
                if (fieldType == typeof(float)) {
                    field.SetValue(row, EditorGUILayout.FloatField((float)field.GetValue(row), GUILayout.Width(200f)));
                    continue;
                }
                if (fieldType == typeof(string)) {
                    string rowValueStr = (string)field.GetValue(row);
                    string textFieldValue = EditorGUILayout.TextField(rowValueStr == null ? "" : rowValueStr, GUILayout.Width(200f));
                    field.SetValue(row, textFieldValue == "" ? null : textFieldValue);
                    continue;
                }
                if (fieldType.IsEnum) {
                    field.SetValue(row, EditorGUILayout.EnumPopup((Enum)field.GetValue(row), GUILayout.Width(200f)));
                    continue;
                }
                if (fieldType == typeof(GameObject)) {
                    field.SetValue(row, EditorGUILayout.ObjectField((GameObject)field.GetValue(row), typeof(GameObject), false, GUILayout.Width(200f)));
                    continue;
                }
                throw new Exception(string.Format("Unknown type: {0}", fieldType.Name));
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
