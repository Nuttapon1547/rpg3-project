using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> monsters;
    public List<Enemy> Monsters
        { get { return monsters; } }

    public static EnemyManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (Character m in monsters)
        {
            m.charInit(VFXManager.instance, UIManager.instance, InventoryManager.Instance);
        }

        InventoryManager.Instance.AddItem(monsters[0], 0);
        InventoryManager.Instance.AddItem(monsters[0], 1);
        InventoryManager.Instance.AddItem(monsters[0], 2);
    }
}
