using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new pickMat", menuName = "Pickable Material")]
public class PickableMaterial : RawMaterial
{
    public bool revivable { get { return timeBeforeRevive != Vector2.zero; } }
    public Vector2 timeBeforeRevive;

}
