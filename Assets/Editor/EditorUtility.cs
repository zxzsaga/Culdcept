using System;
using UnityEditor;
using UnityEngine;

namespace Culdcept {
    public static class EditorUtility {

        /// <summary>
        /// 清空 Console 窗口里的 log
        /// </summary>
        public static void ClearConsoleLog() {
            Type type = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
            System.Reflection.MethodInfo method = type.GetMethod("Clear");
            method.Invoke(null, null);
        }

        /// <summary>
        /// 将 string 转为 enum
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(string value) {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
