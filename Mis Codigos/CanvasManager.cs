using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    // Function to activate a specific panel
    public void ActivatePanel(GameObject panelToActivate)
    {
        if (panelToActivate != null)
            panelToActivate.SetActive(true);
    }

    // Function to deactivate a specific panel
    public void DeactivatePanel(GameObject panelToDeactivate)
    {
        if (panelToDeactivate != null)
            panelToDeactivate.SetActive(false);
    }
}
