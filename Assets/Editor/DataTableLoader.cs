using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Data;
using UnityEditor;
using UnityEngine;
using Excel;

namespace Culdcept {
    public static class DataTableLoader {

        /// <summary>
        /// 读取生物表。
        /// </summary>
        [MenuItem("Culdcept/Excel/Load Creature Data Table")]
        public static void LoadWeaponDataTable() {
            EditorUtility.ClearConsoleLog();
            string filePath = "Assets/XLSX/Culdcept.xlsx";
            string sheetName = "Creature";
            CreatureDataTable dataTable = LoadDataTable<CreatureDataTable, CreatureData>(filePath, sheetName);
            AssetDatabase.CreateAsset(dataTable, "Assets/Resources/Tables/CreatureDataTable.asset");
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 读取 xlsx 文件，生成 ScriptableObject.
        /// </summary>
        /// <typeparam name="Table"></typeparam>
        /// <typeparam name="Row"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private static Table LoadDataTable<Table, Row>(string filePath, string sheetName) where Table : DataTable where Row : new() {
            FieldInfo[] fields = typeof(Row).GetFields();
            DataRowCollection sheetRows = GetRowsFromXlsxSheet(filePath, sheetName); // xlsx sheet 中的数据
            List<Row> rowList = new List<Row>();

            for (int i = 2, rowsCount = sheetRows.Count; i < rowsCount; i++) { // 首行作为 columnName 了，所以0对应 xlsx 里第2行，2对应第4行
                string idStr = sheetRows[i]["id"].ToString();
                if (idStr == "") { // id 为空就终止读取
                    break;
                }
                object row = new Row(); // boxing
                for (int j = 0, fieldsLength = fields.Length; j < fieldsLength; j++) {
                    FieldInfo field = fields[j];
                    SetRow(row, field, sheetRows[i][field.Name]);
                }
                rowList.Add((Row)row); // unboxing
            }

            Table dataTable = ScriptableObject.CreateInstance<Table>();
            dataTable.GetType().GetField("rows").SetValue(dataTable, rowList.ToArray());
            return dataTable;
        }

        /// <summary>
        /// 读取位于 filePath 的 xlsx 文件，将其名为 sheetName 的 sheet 转化为 DataRowCollection 格式输出。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private static DataRowCollection GetRowsFromXlsxSheet(string filePath, string sheetName) {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();
            return result.Tables[sheetName].Rows;
        }

        /// <summary>
        /// 根据 field 的类型将 rowValue 反序列化为对应类型，写入 row 的对应属性。
        /// 每新加一种 Row 成员的类型都要改写这个函数。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        private static void SetRow<T>(T row, FieldInfo field, object value) {
            Type fieldType = field.FieldType;
            // TODO: 强制类型转换可能会报错
            if (fieldType == typeof(bool)) {
                field.SetValue(row, (bool)value);
                return;
            }
            if (fieldType == typeof(int)) {
                field.SetValue(row, (int)(double)value);
                return;
            }
            if (fieldType == typeof(float)) {
                field.SetValue(row, (float)(double)value);
                return;
            }
            if (fieldType == typeof(string)) {
                field.SetValue(row, value.ToString());
                return;
            }
            string valueString = value.ToString();
            if (fieldType.IsEnum) {
                field.SetValue(row, Enum.Parse(fieldType, valueString));
                return;
            }
            if (fieldType == typeof(GameObject)) {
                GameObject gameObject = Resources.Load<GameObject>(valueString);
                if (gameObject == null) {
                    throw new Exception(string.Format("No resources: {0}.", valueString));
                }
                field.SetValue(row, gameObject);
                return;
            }
            throw new Exception(string.Format("Unknown type: {0}", fieldType.Name));
        }
    }
}