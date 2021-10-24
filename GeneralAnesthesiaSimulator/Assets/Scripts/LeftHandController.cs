using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ActionBasedController))]
public class LeftHandController : MonoBehaviour
{
    [Tooltip("Hand associated with this controller")]
    public Hand thisHand;
    public ActionBasedController controller;

    [SerializeField]
    private InputActionReference simulationPressAction;
    [SerializeField]
    private InputActionReference openMenu;

    [SerializeField]
    public MenuManager menuManager;
    public bool isHolding;

    private XRDirectInteractor interactor;
    private bool isPressing;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<ActionBasedController>();
        interactor = gameObject.GetComponent<XRDirectInteractor>();
        this.thisHand = gameObject.GetComponentInChildren<Hand>();
    }

    // Update is called once per frame
    void Update()
    {
        bool menuInputValue = openMenu.action.ReadValue<bool>();
        if (menuManager.IsMenuOpen && menuInputValue)
        {
            menuManager.CloseMenu();
        }
        else if (!menuManager.IsMenuOpen && menuInputValue)
        {
            menuManager.OpenMenu();
        }
        float selectInputValue = controller.selectAction.action.ReadValue<float>();
        this.thisHand.SetGrip(selectInputValue);
        if (selectInputValue > 0.4 && interactor.selectTarget != null)
        {
            isHolding = true;
        }
        else
        {
            isHolding = false;
            if (thisHand.transform.parent != gameObject.transform)
            {
                thisHand.transform.SetParent(gameObject.transform, false);
            }
        }

        float activateInputValue = controller.activateAction.action.ReadValue<float>();
        this.thisHand.SetTrigger(activateInputValue);

        if (activateInputValue > 0.4 && isHolding)
        {
            isPressing = true;
        }
        else
        {
            isPressing = false;
        }
        if (isHolding)
        {
            ValveDial heldDial = interactor.selectTarget.gameObject.GetComponent<ValveDial>();
            if (heldDial != null && thisHand.transform.parent != heldDial.transform)
            {
                thisHand.transform.SetParent(heldDial.transform, false);
            }
        }

        this.thisHand.SetPress(simulationPressAction.action.ReadValue<float>());
    }
}
