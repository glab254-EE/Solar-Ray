using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAttributesManager : MonoBehaviour
{
    [Serializable]
    public struct GlobalAttribute
    {
        [field:SerializeField]
        public string Name{get;internal set;}
        [field:SerializeField]
        public float Value{get;internal set;}
        [field:SerializeField]
        public float Incerment{get;internal set;}
    }
    [field:SerializeField]
    private List<GlobalAttribute> Attributes;
    internal event Action OnAttributeChange;
    public static GlobalAttributesManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public int GetAttribute(string name)
    {
        for (int i = 0; i < Attributes.Count; i++)
        {
            GlobalAttribute attribute = Attributes[i];
            if (attribute.Name == name)
            {
                return i;
            }
        }
        return -1;
    }
    public bool TryAddAttribute(string name)
    {
        int attributeIndex = GetAttribute(name);
        if (attributeIndex != -1)
        {
            GlobalAttribute attribute = Attributes[attributeIndex];
            Attributes.RemoveAt(attributeIndex);
            attribute.Value += attribute.Incerment;
            Attributes.Insert(attributeIndex,attribute);
            return true;
        }
        Debug.Log("Fail");
        OnAttributeChange?.Invoke();
        return false;
    }
    public bool TryGetAttribute(out float output,string name, float def = 0)
    {
        int attributeIndex = GetAttribute(name);
        if (attributeIndex != -1)
        {
            GlobalAttribute attribute = Attributes[attributeIndex];
            output = attribute.Value;
            return true;
        }
        output = def;
        return false;        
    }
}
