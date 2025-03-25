using UnityEngine;

public interface IInteractable
{
    void OnHoverEnter(); // Called when hovered over
    void OnHoverExit();  // Called when not hovered over
    void OnInteract();   // Called when clicked or interacted with
    void Pickup(Transform holdPosition); // Pickup object
    void Drop(); // Drop object

    string GetText();
}