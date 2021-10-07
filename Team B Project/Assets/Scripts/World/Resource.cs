using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    public int amount;
    public enum ResourceKind { metal, fuel };
    public ResourceKind kind;

    public Resource(int amount, ResourceKind kind)
    {
        this.amount = amount;
        this.kind = kind;
    }
}