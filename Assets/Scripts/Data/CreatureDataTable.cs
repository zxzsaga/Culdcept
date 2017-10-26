using System.Collections.Generic;

namespace Culdcept {
    public class CreatureDataTable : DataTable {

        public static CreatureDataTable Instance {
            get {
                if (instance == null) {
                    instance = UnityEngine.Resources.Load<CreatureDataTable>("DataTables/CreatureDataTable");
                    if (instance == null) {
                        throw new System.Exception("No creature data table.");
                    }
                    instance.SetDictionaty();
                }
                return instance;
            }
        }
        private static CreatureDataTable instance;
        public CreatureData this[int index] {
            get {
                return dictionary[index];
            }
        }
        public CreatureData[] rows;
        public Dictionary<int, CreatureData> dictionary;

        private void SetDictionaty() {
            dictionary = new Dictionary<int, CreatureData>();
            if (rows == null) {
                return;
            }
            for (int i = 0, length = rows.Length; i < length; i++) {
                CreatureData row = rows[i];
                dictionary.Add(row.id, row);
            }
        }
    }
}
