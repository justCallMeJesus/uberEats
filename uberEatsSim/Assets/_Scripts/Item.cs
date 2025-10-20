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

    public List<Shelf> possibleShelves = new List<Shelf>();
    private List<Shelf> selectedShelves = new List<Shelf>();
        





    // Start is called before the first frame update
    void Start()
    {
        //Item thisItem = this;
        possibleShelves = FindObjectsOfType<Shelf>().Where(shelf => shelf.shelfSO.items.Contains(this)).ToList();
        
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
                EnableHighlight();

            }
        }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            Debug.Log("player left range");
        }
        DisableHighlight();
    }

    private void EnableHighlight()
    {
        Material[] newMaterials = new Material[2];
        newMaterials[0] = Visual.material;
        newMaterials[1] = highlightMat;
        Visual.materials = newMaterials;
    }

    private void DisableHighlight()
    {
        Material[] newMaterials = new Material[1];
        newMaterials[0] = Visual.material;
        Visual.materials = newMaterials;
    }

    public bool CheckItemList()
    {
        bool matchFound = GameManager.Instance.selectedItems.Any(item => item.itemSO == this.itemSO);
        return matchFound;

        //foreach(Item item in GameManager.Instance.selectedItems)
        //{
        //    if(item.itemSO == itemSO)
        //    {
        //        Debug.Log(itemSO );
        //        Debug.Log(item.itemSO);
        //        return true;
        //    }
        //}
        //return false;
    }
}
