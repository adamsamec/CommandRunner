using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CommandRunner
{
    /// <summary>
    /// Utility class
    /// </summary>
    public static class Utils
    {
        public static object? GetPropValue(object obj, string propName)
        {
            return obj.GetType().GetProperty(propName)?.GetValue(obj, null);
        }

        public static void SetPropValue(object obj, string propName, object? propValue)
        {
            obj.GetType().GetProperty(propName)?.SetValue(obj, propValue, null);
        }

        public static void SetYesOrNo(object obj, object defaultObj, string[] propNames)
        {
            foreach (var propName in propNames)
            {
                var propValue = (string?)GetPropValue(obj, propName);
                var defaultPropValue = (string?)GetPropValue(defaultObj, propName);
                var newPropValue = (propValue == "yes" || propValue == "no") ? propValue : defaultPropValue;
                SetPropValue(obj, propName, newPropValue);
            }
        }

        public static void SetNotNull(object obj, object defaultObj, string[] propNames)
        {
            foreach (var propName in propNames)
            {
                var propValue = (string[]?)GetPropValue(obj, propName);
                var defaultPropValue = (string[]?)GetPropValue(defaultObj, propName);
                var newPropValue = propValue != null ? propValue : defaultPropValue;
                SetPropValue(obj, propName, newPropValue);
            }
        }
    }
}