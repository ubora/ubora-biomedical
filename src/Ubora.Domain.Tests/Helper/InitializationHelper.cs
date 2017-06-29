using System.Reflection;

namespace Ubora.Domain.Tests.Helper
{
    public static class InitializationHelper
    {
        private const BindingFlags AllInstanceBindingFlags =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static void SetPropertyValue(this object instance, string propertyName, object newValue)
        {
            var type = instance.GetType();

            var prop = type.GetProperty(propertyName, AllInstanceBindingFlags);

            prop.SetValue(instance, newValue, null);
        }

        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            return (T)obj.GetType().GetProperty(propertyName, AllInstanceBindingFlags).GetValue(obj);
        }

        public static object InvokeMethod<T>(this T obj, string methodName, params object[] parameters)
        {
            var methodInfo = obj.GetType().GetMethod(methodName, AllInstanceBindingFlags);

            return methodInfo.Invoke(obj, parameters);
        }

    }
}
