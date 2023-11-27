using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnchorType
{
    left,
    right,
}

[SelectionBase]
public class AnchorForBridge : MonoBehaviour
{
    public AnchorForBridge other;
    public AnchorType type;
    public bool isBuilt { get; private set; }

    public void Build()
    {
        isBuilt = other.isBuilt = true;
    }
}
