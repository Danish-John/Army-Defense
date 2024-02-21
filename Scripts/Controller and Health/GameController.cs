using UnityEngine;
using UnityEngine.UI;
using RDG;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] GameObject[] InstantiatingObjectPrefab = null;

    [SerializeField] int[] InstantiatingObjectCost;

    public Slider PowerSlider;

    public int IndexOfInstantiatedObject;

    public GameObject prefab;  // Assign the prefab in the Unity Editor

    public float instantiationDistance = 1.5f;  // Set the fixed distance for instantiation

    private Vector3 lastInstantiationPosition;
    GameObject firstObject;
    bool clickSession, firstObjectRotationStatus;

    public bool isButtonClicked = false;



    private void Awake()
    {
        Instance = this;

    }


    private void Start()
    {
        Time.timeScale = 1;


        clickSession = false;
        firstObjectRotationStatus = false;
        lastInstantiationPosition = transform.position;

        InstantiatingObjectPrefab[0] = InventoryManager.Instance.BarrierPrefab;
        InstantiatingObjectPrefab[1] = InventoryManager.Instance.PolicePrefab;
        InstantiatingObjectPrefab[2] = InventoryManager.Instance.CanonPrefab;



        prefab = InstantiatingObjectPrefab[IndexOfInstantiatedObject];
    }


    public void SetTheValueOfIndex(int index)
    {
        IndexOfInstantiatedObject = index;

        if (index == 0)
        {
            prefab = InstantiatingObjectPrefab[IndexOfInstantiatedObject];

        }
        else if (index == 1)
        {
            prefab = InstantiatingObjectPrefab[IndexOfInstantiatedObject];

        }
        if (index == 2)
        {
            prefab = InstantiatingObjectPrefab[IndexOfInstantiatedObject];
        }
    }


    public void setBoolforButtons(bool check)
    {
        isButtonClicked = check;
    }

    private void Update()
    {
        if (!isButtonClicked)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                RaycastHit hit2;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Player" || hit.collider.tag == "Barrier" || hit.collider.tag == "Canon" || hit.collider.tag == "Obstacle")
                    {
                        return;
                    }
                    Vector3 mousePosition = hit.point;
                    if (clickSession)
                        SetFirstObjectRotaion(mousePosition);
                    if (!clickSession)
                    {
                        InstantiatePrefab(mousePosition);
                        clickSession = true;
                        // return;
                    }
                    float dis = Vector3.Distance(mousePosition, lastInstantiationPosition);

                    if (dis >= instantiationDistance)
                    {
                        int numberOfObjects = Mathf.FloorToInt(dis / instantiationDistance);
                        if (numberOfObjects > 1)
                        {
                            for (int i = 0; i < numberOfObjects; i++)
                            {
                                Vector3 direction = (mousePosition - lastInstantiationPosition).normalized;
                                Vector3 newPosition = lastInstantiationPosition + direction * instantiationDistance;
                                if (Physics.Raycast(ray, out hit2))
                                {
                                    if (hit2.collider.tag != "Player" || hit2.collider.tag != "Barrier" || hit2.collider.tag != "Canon")
                                    {
                                        InstantiatePrefab(newPosition);
                                    }
                                    else
                                    {
                                        lastInstantiationPosition = lastInstantiationPosition + direction * instantiationDistance;
                                    }
                                }
                            }
                        }
                        else
                        {
                            InstantiatePrefab(mousePosition);
                        }
                    }

                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                clickSession = false;
                firstObjectRotationStatus = false;

            }
        }
        else
            return;
        
    }

    void InstantiatePrefab(Vector3 position)
    {

        Vibration.Vibrate(100, 50);

        if (!clickSession)
        {
            if (PowerSlider.value >= InstantiatingObjectCost[IndexOfInstantiatedObject])
            {
                firstObject = Instantiate(prefab, position, Quaternion.identity);
                PowerSlider.value -= InstantiatingObjectCost[IndexOfInstantiatedObject];
                firstObjectRotationStatus = true;
                UIHandlingScript.Instance.UpdatePowerText(PowerSlider.value.ToString());
            }
        }
        else
        {
            if (PowerSlider.value >= InstantiatingObjectCost[IndexOfInstantiatedObject])
            {
                Instantiate(prefab, position, Quaternion.LookRotation(position - lastInstantiationPosition));
                PowerSlider.value -= InstantiatingObjectCost[IndexOfInstantiatedObject];
                UIHandlingScript.Instance.UpdatePowerText(PowerSlider.value.ToString());
                firstObjectRotationStatus = false;
            }
        }
        lastInstantiationPosition = position;
    }

    void SetFirstObjectRotaion(Vector3 pos)
    {
        if (firstObjectRotationStatus)
        {
            firstObject.transform.rotation = Quaternion.LookRotation(pos - firstObject.transform.position);
        }
    }

}


