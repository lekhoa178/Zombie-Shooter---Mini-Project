using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public InputMaster InputActions { get; private set; }
    public InputMaster.PlayerActions PlayerActions { get; private set; }

    private void Awake()
    {
        InputActions = new InputMaster();

        PlayerActions = InputActions.Player;
    }

    private void OnEnable()
    {
        InputActions.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }

    public void DisableActionFor(InputAction action, float seconds)
    {
        StartCoroutine(DisableAction(action, seconds));
    }

    private IEnumerator DisableAction(InputAction action, float seconds)
    {
        action.Disable();

        yield return new WaitForSeconds(seconds);

        action.Enable();
    }
}
