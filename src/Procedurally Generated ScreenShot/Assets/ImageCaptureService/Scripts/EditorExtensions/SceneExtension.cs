using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Provides extension methods for the Scene class.
/// </summary>
public static class SceneExtension
{
    /// <summary>
    /// Gets the first component of type T found in the root game objects of the scene or their children.
    /// </summary>
    /// <typeparam name="T">The type of component to find.</typeparam>
    /// <param name="scene">The scene to search in.</param>
    /// <returns>The found component or null if no component of type T is found.</returns>
    public static T GetComponent<T>(this Scene scene) where T : Component
    {
        T componentResult = null;

        GameObject[] sceneGameObjects = scene.GetRootGameObjects();

        foreach (GameObject sceneGameObject in sceneGameObjects)
        {
            componentResult = sceneGameObject.GetComponent<T>();

            if (componentResult != null)
            {
                return componentResult;
            }

            componentResult = sceneGameObject.GetComponentInChildren<T>();

            if (componentResult != null)
            {
                return componentResult;
            }
        }

        return componentResult;
    }
}
