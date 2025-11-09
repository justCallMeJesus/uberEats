using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{

    [SerializeField] public ItemSO itemSO;
    [SerializeField] private MeshRenderer Visual;
    [SerializeField] private Material highlightMat;
    [SerializeField] private Material closestHighlightMat;

    public List<Shelf> possibleShelves = new List<Shelf>();
    private List<Shelf> selectedShelves = new List<Shelf>();

    public bool ItemSelected = false;
        





    // Start is called before the first frame update
    void Start()
    {
        //Item thisItem = this;
        //possibleShelves = FindObjectsOfType<Shelf>().Where(shelf => shelf.shelfSO.items.Contains(this)).ToList();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        //int randomIndex = Random.Range(0, possibleShelves.Count-1);

        //Shelf randomItem = possibleShelves[randomIndex];

        //possibleShelves.Remove(randomItem);

        //selectedShelves.Add(randomItem);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerMovement player))
        {
            Debug.Log("player in range");
            if (CheckItemList())
            {
                EnableNormalHighlight();

            }
        }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            Debug.Log("player left range");
        }
        DisableNormalHighlight();
    }

    public void EnableNormalHighlight()
    {
        Material[] newMaterials = new Material[2];
        newMaterials[0] = Visual.sharedMaterial;
        newMaterials[1] = highlightMat;
        Visual.materials = newMaterials;
    }

    public void DisableNormalHighlight()
    {
        Material[] newMaterials = new Material[1];
        newMaterials[0] = Visual.sharedMaterial;
        Visual.materials = newMaterials;
    }

    public void EnableClosestHighlight()
    {
        Material[] newMaterials = new Material[2];
        newMaterials[0] = Visual.sharedMaterial;
        newMaterials[1] = closestHighlightMat;
        Visual.materials = newMaterials;
    }

    public bool CheckItemList()
    {
        // check if selectedItems has an item with same itemSO as this items ItemSO
        bool matchFound = GameManager.Instance.selectedItems.Any(selectedItem => selectedItem.itemSO == this.itemSO);
        return matchFound;
    }
}
