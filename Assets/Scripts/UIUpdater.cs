using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIUpdater : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI acorns;

    //Squirrel Menu
    [Header("Squirrel")]
    [SerializeField] Sprite squirrelHover;
    [SerializeField] Sprite squirrelHoverDefault;
    [SerializeField] Sprite squirrelClick;
    [SerializeField] Image squirrelButtonImage;
    [SerializeField] GameObject menuSquirrel;
    [SerializeField] GameObject menuTree;
    [SerializeField] GameObject[] buyableSquirrels;
    [SerializeField] Squirrels[] squirrels;
    private int[] squirrelQuantity= new int[4];
    private int currentIndexSquirrels = 0;

    //Tree Menu
    [Header("Tree")]
    [SerializeField] Sprite treeClick;
    [SerializeField] Sprite treeDefaultSprite;
    [SerializeField] Image treeButtonImage;

    [Header("Menus")]
    GraphicRaycaster raycaster;
    EventSystem eventSystem;
    PointerEventData eventMouse;


    private void Start()
    {
        for (int i = 0; i < squirrelQuantity.Length; i++)
            squirrelQuantity[i] = 1;
        EventManager.AddListener("UpdateAcornUI", UpdateAcorns);
        eventSystem = GetComponent<EventSystem>();
        raycaster = gameObject.GetComponent<GraphicRaycaster>();

        UpdateShopSquirrels();
    }

    private void Update()
    {
        if(PlayerInput._Instance.OnClick())
        {
            CheckForMenus();
        }
    }

    private void CheckForMenus()
    {
        //If we do we get the position of our mouse on screen
        Vector2 mousePos = Mouse.current.position.ReadValue();
        eventMouse = new PointerEventData(eventSystem);
        eventMouse.position = mousePos;

        //A ray using position of our mouse, we use this to look where we are pointing
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventMouse, results);
        if (results.Count == 0)
        {
            if(menuSquirrel.activeInHierarchy)
                OnClickSquirrel();
            if (menuTree.activeInHierarchy)
                OnClickTree();
        }
        
    }

    private void UpdateAcorns(object acornsSize)
    {
        string currentAcorns = ((int)acornsSize).ToString();
        acorns.text = currentAcorns;
    }

    public void HoverSquirrelMenuEnter()
    {
        squirrelButtonImage.sprite = squirrelHover;
    }

    public void HoverSquirrelMenuExit()
    {
        squirrelButtonImage.sprite = squirrelHoverDefault;
    }

    public void OnClickSquirrel()
    {
        squirrelButtonImage.sprite = squirrelClick;
        if(menuSquirrel.activeInHierarchy)
            menuSquirrel.SetActive(false);
        else
            menuSquirrel.SetActive(true);

        if (menuTree.activeInHierarchy)
            menuTree.SetActive(false);

        StartCoroutine(ChangeSprite(squirrelButtonImage, squirrelHoverDefault));
    }

    public void OnPointEnterSquirrel()
    {
        squirrelButtonImage.sprite = squirrelClick;
    }

    private IEnumerator ChangeSprite(Image image, Sprite sprite)
    {
        yield return new WaitForSeconds(.1f);
        image.sprite = sprite;

    }

    public void OnClickTree()
    {
        treeButtonImage.sprite = treeClick;
        if (menuSquirrel.activeInHierarchy)
            menuSquirrel.SetActive(false);

        if (!menuTree.activeInHierarchy)
            menuTree.SetActive(true);
        else
            menuTree.SetActive(false);
    }

    public void HoverTreeMenuExit()
    {
        treeButtonImage.sprite = treeDefaultSprite;
    }

    public void OnNextSquirrelsPress()
    {
        if (currentIndexSquirrels > squirrels.Length || GameManager.currentLevel < (int)((currentIndexSquirrels+4)/4))
            return;
        currentIndexSquirrels += 4;
        UpdateShopSquirrels();
    }

    public void OnPriorSquirrelsPress()
    {
        if (currentIndexSquirrels <= 0)
            return;

        currentIndexSquirrels -= 4;
        UpdateShopSquirrels();
    }

    private void UpdateShopSquirrels()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i + currentIndexSquirrels == squirrels.Length)
                break;

            UpdateShopSingleSquirrel(buyableSquirrels[i], squirrels[i + currentIndexSquirrels]);
        }

        for (int i = 0; i < 4; i++)
        {
            foreach (Transform item in buyableSquirrels[i].transform)
            {
                if (item.name == "Description")
                    if (item.GetComponent<TextMeshProUGUI>().text == "Sample text")
                        buyableSquirrels[i].SetActive(false);
                    else
                        buyableSquirrels[i].SetActive(true);
            }

            if (squirrels.Length - 1 < i + currentIndexSquirrels || squirrels[i + currentIndexSquirrels].requiredLevel > GameManager.currentLevel )
                buyableSquirrels[i].SetActive(false);
        }
    }

    private void UpdateShopSingleSquirrel(GameObject squirrel, Squirrels squirrelInfo)
    {
        squirrel.GetComponentInChildren<TextMeshProUGUI>().text = squirrelInfo.description + " Price: " + squirrelInfo.defaultPrice + " Acorns";
        squirrel.GetComponent<Image>().color = squirrelInfo.color;
        Button buy = null;
        foreach (Transform child in squirrel.transform)
        {
            if (child.name == "BuyButton")
            {
                buy = child.GetComponent<Button>();
                break;
            }
        }

        if (buy != null)
        {
            buy.onClick.AddListener(delegate() { BoughtSquirrel(squirrelInfo, squirrelInfo.index - currentIndexSquirrels); });
        }
    }

    public void BoughtSquirrel(Squirrels squirrel, int index)
    {
        for(int i = 0; i < squirrelQuantity[index]; i++)
            EventManager.TriggerEvent("BoughtSquirrel", squirrel);
        squirrelQuantity[index] = 1;
        foreach (Transform child in buyableSquirrels[index].transform)
        {
            if (child.name == "Price")
            {
                child.GetComponent<TextMeshProUGUI>().text = squirrelQuantity[index].ToString();
                break;
            }
        }
    }

    public void AddAmountToBuy(int index)
    {
        if (squirrelQuantity[index] == 99)
            return;
        squirrelQuantity[index] += 1;
        foreach(Transform child in buyableSquirrels[index].transform)
        {
            Debug.Log(child.name);
            if(child.name == "Price")
            {
                Debug.Log(squirrelQuantity[index]);
                child.GetComponent<TextMeshProUGUI>().text = squirrelQuantity[index].ToString();
                break;
            }
        }
    }

    public void DecreaseAmountToBuy(int index)
    {
        if (squirrelQuantity[index] == 1)
            return;

        squirrelQuantity[index] -= 1;
        foreach (Transform child in buyableSquirrels[index].transform)
        {
            if (child.name == "Price")
            {
                child.GetComponent<TextMeshProUGUI>().text = squirrelQuantity[index].ToString();
                break;
            }
        }
    }
}
