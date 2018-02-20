//#define CSHARP_7

using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class GameObjectExtension {
    /// <summary>
    /// Get All level children gameobjects of gameobjects
    /// </summary>
    /// <param name="_go"></param>
    /// <returns>Array of all childrens gamobjects</returns>
    public static GameObject[] GetGameObjectOfAllChild(this GameObject _go) {
        return GetChildOfTransform(_go.transform).ToArray();
    }

    /// <summary>
    /// Recurse call to get all the child of gameobjects 
    /// </summary>
    /// <param name="_transform"></param>
    /// <returns></returns>
    private static List<GameObject> GetChildOfTransform(Transform _transform) {
        List<GameObject> result = new List<GameObject>();
        foreach (Transform transform in _transform) {
            result.Add(transform.gameObject);
            if (transform.childCount >= 0) {
                result.AddRange(GetChildOfTransform(transform));
            }
        }
        return result;
    }

    /// <summary>
    /// Get the giving game object full path in scene
    /// </summary>
    /// <param name="_go"></param>
    /// <returns></returns>
    public static string GetGameObjectFullPath(this GameObject _go) {
        return GetGoFullPath(_go.transform);
    }

    /// <summary>
    /// Rescurse call to get Full Path of Gameobject in scene
    /// </summary>
    /// <param name="_transform"></param>
    /// <returns></returns>
    private static string GetGoFullPath(Transform _transform) {
        string goPath = "";
        if (_transform.parent == null) {
            goPath += _transform.name;
        }
        else {
            goPath += _transform.name;
#if CSHARP_7
            goPath = $"{GetGoFullPath(_transform.parent)}/{goPath}";
#else
            goPath = new StringBuilder(GetGoFullPath(_transform.parent))
                .Append("/")
                .Append(goPath).ToString();
#endif
        }
        return goPath;
    }
    
    private static List<Component> GetComponentInChildrenAtLevel(this GameObject _go, int _childLevel, Type _componentType, bool _isGetFromAllChild = true){
        List<Component> result = null;
        string goPath = "";
        goPath = GetGoFullPath(_go.transform);
        StringBuilder strBd = new StringBuilder();
        if (!string.isNullOrEmpty(goPath)){
            string[] path = goPath.Split('/');
            // if path lengh less or equal _childLevel that's mean the child of this gameobject not exist in this childLevel
            if (path == null || path.lengh <= _childLevel){
                return null;
            }
            
            for (int i = 0; i < _childLevel; i++){
                strBd.Append(path[i]);
                if (i != _childLevel -1 ){
                    goPath.Append("/");
                }
            }
            //_isGetFromAllChild and component type will implement later
            Transform child = _go.transform.Find(strBd.ToString());
            if (child != null){
                result = child.GetComponents<Component>().ToList();
            }
        }
        return result;
    }
}
