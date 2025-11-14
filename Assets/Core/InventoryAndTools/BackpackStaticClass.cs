using System.Collections.Generic;
using UnityEngine;

public sealed class BackpackStaticClass
{
    internal List<ATool> backpack;
    private static BackpackStaticClass privateInstace = null;
    public static BackpackStaticClass Instance
    {
        get
        {
            privateInstace ??= new();
            return privateInstace;
        }
    }
    private BackpackStaticClass()
    {
        backpack = new();
    }
    internal void ResetBackpack()
    {
        backpack = new();
    }
    internal void ResetBackpack(List<ATool> desiredPack)
    {
        backpack = desiredPack;
    }
}
