using UnityEngine;
using UnityEngine.EventSystems;

public class LeftClick : MonoBehaviour
{
    public static LeftClick instance;
    private Camera cam;
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private RectTransform boxSelection;
    private Vector2 oleAnchoredPos;
    private Vector2 startPos;
    private void Awake()
    {
        if (instance != null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        cam = Camera.main;
        layerMask = LayerMask.GetMask("Ground", "Character", "Building", "Item");
        boxSelection = UIManager.instance.SelectionBox;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            ClearEverything();
        }
        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            UpdateSelectionBox(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox(Input.mousePosition);
            TrySelect(Input.mousePosition);
        }
    }
    private void SelectCharacter(RaycastHit hit)
    {
        Character hero = hit.collider.GetComponent<Character>();
        Debug.Log("Selected Char: " + hit.collider.gameObject);
        
        PartyManager.instance.SelectChars.Add(hero);
        hero.ToggleRingSelection(true);
        UIManager.instance.ShowMagicToggles();
    }
    private void TrySelect(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            switch (hit.collider.tag)
            {
                case "Player":
                case "Hero":
                    SelectCharacter(hit);
                    break;
            }
        }
    }
    public void ClearRingSelection()
    {
        foreach (Character c in PartyManager.instance.SelectChars)
        {
            c.ToggleRingSelection(false);
        }
    }
    private void ClearEverything()
    {
        ClearRingSelection();
        PartyManager.instance.SelectChars.Clear();
    }
    private void UpdateSelectionBox(Vector2 mousePos)
    {
        if (!boxSelection.gameObject.activeInHierarchy)
        {
            boxSelection.gameObject.SetActive(true);
        }
        float width = mousePos.x - startPos.x;
        float hight = mousePos.y - startPos.y;
        boxSelection.anchoredPosition = startPos + new Vector2(width / 2, hight / 2);
        width = Mathf.Abs(width);
        hight = Mathf.Abs(hight);
        boxSelection.sizeDelta = new Vector2(width, hight);
        oleAnchoredPos = boxSelection.anchoredPosition;
    }
    private void ReleaseSelectionBox(Vector2 mousePos)
    {
        Vector2 corner1, corner2;
        boxSelection.gameObject.SetActive(false);
        corner1 = oleAnchoredPos - (boxSelection.sizeDelta / 2);
        corner2 = oleAnchoredPos + (boxSelection.sizeDelta / 2);
        foreach (Character member in PartyManager.instance.Members)
        {
            Vector2 unitPos = cam.WorldToScreenPoint(member.transform.position);
            if ((unitPos.x > corner1.x && unitPos.x < corner2.x) && (unitPos.y > corner1.y && unitPos.y < corner2.y))
            {
                PartyManager.instance.SelectChars.Add(member);
                member.ToggleRingSelection(true);
            }
        }
        boxSelection.sizeDelta = new Vector2(0, 0);
    }
}