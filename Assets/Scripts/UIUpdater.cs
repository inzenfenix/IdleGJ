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
    [SerializeField] GameObject[] buyableSquirrels;
    [SerializeField] Squirrels[] squirrels;
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
        if (results.Count == 0 && menuSquirrel.activeInHierarchy)
        {
            OnClickSquirrel();
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
    }

    public void HoverTreeMenuExit()
    {
        treeButtonImage.sprite = treeDefaultSprite;
    }

    public void OnNextSquirrelsPress()
    {
        //We have an index for each new squirrel, what we could do is to replace them
        //With the next batch that we send from the GameManager
        currentIndexSquirrels += 4;
    }

    public void OnPriorSquirrelsPress()
    {
        //We have an index for each new squirrel, what we could do is to replace them
        //With the next batch that we send from the GameManager
        if(currentIndexSquirrels > 0)
            currentIndexSquirrels -= 4;
    }

    private void UpdateShopSquirrels()
    {
        for(int i = 0; i < 4; i++)
        {
            if (i + currentIndexSquirrels == squirrels.Length)
                break;

            UpdateShopSingleSquirrel(buyableSquirrels[i], squirrels[i + currentIndexSquirrels]);
        }

        for (int i = 0; i < 4; i++)
        {
            foreach(Transform item in buyableSquirrels[i].transform)
            {
                if (item.name == "Description")
                    if (item.GetComponent<TextMeshProUGUI>().text == "Sample text")
                        buyableSquirrels[i].SetActive(false);
                    else
                        buyableSquirrels[i].SetActive(true);
            }
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
            buy.onClick.AddListener(delegate() { BoughtSquirrel(squirrelInfo); });
        }
    }

    public void BoughtSquirrel(Squirrels squirrel)
    {
        EventManager.TriggerEvent("BoughtSquirrel", squirrel);
    }
}
