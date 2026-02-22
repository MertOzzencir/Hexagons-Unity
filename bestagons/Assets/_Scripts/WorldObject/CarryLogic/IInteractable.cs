
using UnityEngine;

public interface IInteractable
{
    public void OnPicked();
    public void Carry(Vector3 toPosition);
    public void Drop();
    public void LocomotionLogic();

}
