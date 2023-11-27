using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnchorType
{
    left,
    right,
}

public class AnchorForBridge : MonoBehaviour
{
    public AnchorForBridge other;
    public AnchorType type;
}
