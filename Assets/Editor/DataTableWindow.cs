using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Culdcept {
    public class DataTableWindow : EditorWindow {

        /// <summary>
        /// 添加新表时需要动态修改。
        /// </summary>
        private void OnGUI() {
            bool isDataTable = Selection.activeObject is DataTable;
            if (!isDataTable) {
                return;
            }
            object table = Selection.activeObject;
            string text = DataTableObjectToString(table);
            EditorGUILayout.TextArea(text);
        }

        private static string DataTableObjectToString(object table) {
            if (table is CreatureDataTable) {
                return BuildRawDataTable<CreatureDataTable, CreatureData>(table);
            }
            return "";
        }

        private static string BuildRawDataTable<Table, Row>(object table) where Table : DataTable {
            StringBuilder stringBuilder = new StringBuilder();
            System.Type tableType = table.GetType();
            FieldInfo[] rowFields = typeof(Row).GetFields();
            Row[] rows = (Row[])tableType.GetField("rows").GetValue(table);
            for (int i = 0, length = rowFields.Length; i < length; i++) {
                stringBuilder.Append(rowFields[i].Name + '\t');
            }
            stringBuilder.Append('\n');
            for (int i = 0, rowLength = rows.Length; i < rowLength; i++) {
                Row row = rows[i];
                for (int j = 0, fieldLength = rowFields.Length; j < fieldLength; j++) {
                    FieldInfo field = rowFields[j];
                    System.Type fieldType = field.FieldType;
                    if (fieldType == typeof(GameObject)) {
                        stringBuilder.Append(((GameObject)field.GetValue(row)).name);
                    } else {
                        stringBuilder.Append(field.GetValue(row));
                    }
                    stringBuilder.Append('\t');
                }
                stringBuilder.Append('\n');
            }
            return stringBuilder.ToString();
        }

        [MenuItem("Assets/RawDataTable", true)]
        private static bool ShowRawDataTableValidation() {
            return Selection.activeObject is DataTable;
        }
        [MenuItem("Assets/RawDataTable")]
        private static void ShowRawDataTable() {
            GetWindow<DataTableWindow>();
        }
    }
}
