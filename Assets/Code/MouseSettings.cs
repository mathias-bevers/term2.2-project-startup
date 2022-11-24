using UnityEngine;

[System.Serializable]
public struct MouseSettings 
{
    [SerializeField] float _sensitivityX;
    [SerializeField] float _sensitivityY;
    [SerializeField] bool _invertX;
    [SerializeField] bool _invertY;

    public float sensitivityX { get { return _sensitivityX; } }
    public float sensitivityY { get { return _sensitivityY; } }
    public float invertX { get { return _invertX ? -1 : 1;} }
    public float invertY { get { return _invertY ? -1 : 1; } }
}
