using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActionMap_Handler : MonoBehaviour
{
    [Header("Input System")]
    public InputActionAsset m_InputSchemes;


    public enum ActionMaps
    {
        None, 
        EditMode,
        Gameplay,
        UI
    }

    public ActionMaps selectedActionMap;

    private ActionMaps activeActionMap;

    [Header("Unity Event")]
    public UnityEvent<ActionMaps> OnActionMapChanged;

    [SerializeField] private InputActionMap[] availableActionMaps;

    void Start()
    {
        GetActionMaps();
    }

    private void CheckActionMapChange()
    {
        if (activeActionMap != selectedActionMap)
        {
            activeActionMap = selectedActionMap;
            OnActionMapChanged?.Invoke(activeActionMap);
        }
    }

    void Update()
    {
        CheckActionMapChange();
    }

    void GetActionMaps()
    {
        availableActionMaps = m_InputSchemes.actionMaps.ToArray();
        foreach (InputActionMap actionMap in availableActionMaps)
        {
            actionMap.Disable();  // Disabilita tutte le action map all'inizio
        }
    }

    public void ChangeActionMap()
    {
        string enabledActionMap = "Nessuna mappa";
        switch (activeActionMap)
        {
            case ActionMaps.None:
                break;

            case ActionMaps.EditMode:
                availableActionMaps[0].Enable();
                enabledActionMap = availableActionMaps[0].name;
                break;

            case ActionMaps.Gameplay:
                availableActionMaps[1].Enable();
                enabledActionMap = availableActionMaps[1].name;
                break;

            case ActionMaps.UI:
                availableActionMaps[2].Enable();
                enabledActionMap = availableActionMaps[2].name;
                break;
        }

        Debug.Log($"{enabledActionMap} abilitata");
    }
}
