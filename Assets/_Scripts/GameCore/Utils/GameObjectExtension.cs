using UnityEngine;

namespace Utils.Extensions
{
    public static class GameObjectExtension
    {
        public static T GetOrAddComponent<T>(this GameObject self)
            where T : Component
        {
            if (self.TryGetComponent<T>(out var component) == false)
            {
                component = self.AddComponent<T>();
            }

            return component;
        }

        public static T GetOrAddComponent<T>(this Component self)
            where T : Component
        {
            if (self.TryGetComponent<T>(out var component) == false)
            {
                component = self.gameObject.AddComponent<T>();
            }

            return component;
        }
        public static T SetActive<T>(this T self) where T : Component
        {
            self.gameObject.SetActive(true);
            return self;
        }

        public static T SetInactive<T>(this T self) where T : Component
        {
            self.gameObject.SetActive(false);
            return self;
        }

        public static T SetActive<T>(this T self, bool active) where T : Component
        {
            self.gameObject.SetActive(active);
            return self;
        }
        public static void HideInHierarchy(this GameObject gameObject) {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
        // public static void EnableChildren(this GameObject gameObject) {
        //     gameObject.transform.EnableChildren();
        // }
        // public static void DisableChildren(this GameObject gameObject) {
        //     gameObject.transform.DisableChildren();
        // }
    }
}