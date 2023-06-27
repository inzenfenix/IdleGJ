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
    private int[] squirrelPrices = new int[4];
    private int currentIndexSquirrels = 0;

    //Tree Menu
    [Header("Tree")]
    [SerializeField] Sprite treeClick;
    [SerializeField] Sprite treeDefaultSprite;
    [SerializeField] Image treeButtonImage;
    [SerializeField] TextMeshProUGUI totalAcorns;
    [SerializeField] Slider[] fillBars;
    [SerializeField] GameObject[] getButtons;
    private bool[] availableToLevelUp = new bool[4];
    private bool[] usedLevelUp = new bool[4];
    [SerializeField] Sprite greenGetSprite;
    [SerializeField] Sprite grayGetSprite;
    [SerializeField] int capLevel1;
    [SerializeField] int capLevel2;
    [SerializeField] int capLevel3;
    [SerializeField] int capLevel4;

    [Header("Menus")]
    GraphicRaycaster raycaster;
    EventSystem eventSystem;
    PointerEventData eventMouse;
    [SerializeField] GameObject startMenu;


    private void Start()
    {
        for (int i = 0; i < squirrelQuantity.Length; i++)
        {
            squirrelQuantity[i] = 1;
            squirrelPrices[i] = squirrels[i].defaultPrice;
        }

        for(int i = 0;i < 4; i++)
        {
            availableToLevelUp[i] = false;
            availableToLevelUp[i] = false;
        }

        EventManager.AddListener("UpdateAcornUI", UpdateAcorns);
        EventManager.AddListener("UpdateAcornUI", UpdateAmountOfAcorns);
        EventManager.AddListener("LevelUpTree", UpdateShopSquirrels);
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

    private void UpdateAmountOfAcorns()
    {
        totalAcorns.text = "TOTAL ACORNS: " + GameManager.acornsGrabbed.ToString();
        fillBars[0].value = (float)GameManager.acornsGrabbed / capLevel1;

        if (GameManager.acornsGrabbed / capLevel1 >= 1.0f && !usedLevelUp[0] && !availableToLevelUp[0])
            LevelUpButtonAvailable(0);

        fillBars[1].value = (float)GameManager.acornsGrabbed / capLevel2;

        if (GameManager.acornsGrabbed / capLevel2 >= 1.0f && !usedLevelUp[1] && !availableToLevelUp[1])
            LevelUpButtonAvailable(1);

        fillBars[2].value = (float)GameManager.acornsGrabbed / capLevel3;

        if (GameManager.acornsGrabbed / capLevel3 >= 1.0f && !usedLevelUp[2] && !availableToLevelUp[2])
            LevelUpButtonAvailable(2);

        fillBars[3].value = (float)GameManager.acornsGrabbed / capLevel4;

        if (GameManager.acornsGrabbed / capLevel4 >= 1.0f && !usedLevelUp[3] && !availableToLevelUp[3])
            LevelUpButtonAvailable(3);
    }

    private void LevelUpButtonAvailable(int index)
    {
        getButtons[index].GetComponent<Image>().sprite = greenGetSprite;
        availableToLevelUp[index] = true;
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

    private void UpdateAcorns()
    {
        string currentAcorns = (GameManager.currentAcorns).ToString();
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

        StartCoroutine(ChangeSprite(treeButtonImage, treeDefaultSprite));
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
        squirrel.GetComponentInChildren<TextMeshProUGUI>().text = squirrelInfo.description + " Price: " +
                                                                  squirrelPrices[squirrelInfo.index - currentIndexSquirrels] + " Acorns";
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
            buy.onClick.RemoveAllListeners();
            buy.onClick.AddListener(delegate() { BoughtSquirrel(squirrelInfo, squirrelInfo.index - currentIndexSquirrels); });
        }
    }

    public void BoughtSquirrel(Squirrels squirrel, int index)
    {
        if (squirrelPrices[index] * squirrelQuantity[index] > GameManager.currentAcorns)
        {
            return;
        }
        for(int i = 0; i < squirrelQuantity[index]; i++)
            EventManager.TriggerEvent("BoughtSquirrel", squirrel);

        GameManager.currentAcorns -= squirrelPrices[index] * squirrelQuantity[index];
        squirrelPrices[index] += (int)(squirrelQuantity[index] * squirrelPrices[index])/2 + 
                                 (int)Mathf.Log10(squirrelPrices[index] * squirrelPrices[index] + squirrelQuantity[index]);
        squirrelQuantity[index] = 1;

        foreach (Transform child in buyableSquirrels[index].transform)
        {
            if (child.name == "Quantity")
            {
                child.GetComponent<TextMeshProUGUI>().text = squirrelQuantity[index].ToString();
                break;
            }
        }

        //squirrelPrices[index] = (int)Mathf.Pow(squirrelPrices[index],1.1f + squirrelQuantity[index]/500f);
        UpdateShopSingleSquirrel(buyableSquirrels[index], squirrel);
        UpdateAcorns();
    }

    public void AddAmountToBuy(int index)
    {
        if (squirrelQuantity[index] == 99)
            return;
        squirrelQuantity[index] += 1;
        foreach(Transform child in buyableSquirrels[index].transform)
        {
            if(child.name == "Quantity")
            {
                child.GetComponent<TextMeshProUGUI>().text = squirrelQuantity[index].ToString();
                break;
            }
        }
        buyableSquirrels[index].GetComponentInChildren<TextMeshProUGUI>().text = squirrels[index].description + " Price: " +
                                                                  squirrelPrices[squirrels[index].index - currentIndexSquirrels] * 
                                                                  squirrelQuantity[index] + " Acorns";
    }

    public void DecreaseAmountToBuy(int index)
    {
        if (squirrelQuantity[index] == 1)
            return;

        squirrelQuantity[index] -= 1;
        foreach (Transform child in buyableSquirrels[index].transform)
        {
            if (child.name == "Quantity")
            {
                child.GetComponent<TextMeshProUGUI>().text = squirrelQuantity[index].ToString();
                break;
            }
        }
        buyableSquirrels[index].GetComponentInChildren<TextMeshProUGUI>().text = squirrels[index].description + " Price: " +
                                                                  squirrelPrices[squirrels[index].index - currentIndexSquirrels] *
                                                                  squirrelQuantity[index] + " Acorns";
    }

    public void LevelUp(int index)
    {
        if (availableToLevelUp[index])
        {
            getButtons[index].GetComponent<Image>().sprite = grayGetSprite;
            EventManager.TriggerEvent("LevelUp");
            usedLevelUp[index] = true;
            availableToLevelUp[index] = false;
        }
    }
    
    public void StartGame()
    {
        startMenu.SetActive(false);
    }
}
