using UnityEngine;

public abstract class Bird_BaseState
{
    public abstract void OnEnter(Bird bird);
    public abstract void Update(Bird bird);
    public abstract void OnExit(Bird bird);
    public abstract void OnTriggerEnter2D(Bird bird, Collider2D collision);
    public abstract void OnTriggerStay2D(Bird bird, Collider2D collision);
    public abstract void OnTriggerExit2D(Bird bird, Collider2D collision);
}
