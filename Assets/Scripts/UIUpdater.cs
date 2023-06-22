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
}
