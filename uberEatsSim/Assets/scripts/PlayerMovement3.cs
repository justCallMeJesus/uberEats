using UnityEngine;

public class PlayerMovement3: MonoBehaviour
{
    public float speed = 5f;
    public GameObject ShoppingList;
    public GameObject ListPickUpCheck;

    public Vector3 AtoB;
    [SerializeField] private LayerMask layerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ListPickUpCheck.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AtoB = ShoppingList.transform.position - transform.position;
            if (Physics.Raycast(transform.position, AtoB, out RaycastHit raycastHit, 1f, layerMask))
            {
                if (raycastHit.transform.gameObject == ShoppingList)
                {
                    ShoppingList.SetActive(false);
                    ListPickUpCheck.SetActive(true);


                }
            }
        }
        {
            
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
            {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }   
        {
            
        }
    }
}
