using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public bool puedeSeguir;
    // Function to activate a specific panel
    private void Start()
    {
        puedeSeguir = true;
    }

    public void SetPuedeSeguir(bool booleano)
    {
        puedeSeguir = booleano;
    }

    public void ActivatePanel(GameObject panelToActivate)
    {
        if (panelToActivate != null && puedeSeguir)
            panelToActivate.SetActive(true);
    }

    // Function to deactivate a specific panel
    public void DeactivatePanel(GameObject panelToDeactivate)
    {
        if (panelToDeactivate != null && puedeSeguir)
        {
            panelToDeactivate.SetActive(false);
            
        }
        puedeSeguir = true;
    }

    public void DeactivateAviso(GameObject panelToDeactivate)
    {
        if (panelToDeactivate != null)
        {
            panelToDeactivate.SetActive(false);

        }
    }
}
