using UnityEngine;

public class TMono : MonoBehaviour
{
    void Reset()
    {
        this.LoadComponents();
        this.LoadValues();
    }
    void Awake()
    {
        this.LoadComponents();
        this.LoadValues();
    }
    protected virtual void LoadComponents()
    {
    }
    protected virtual void LoadValues()
    {
    }
}
