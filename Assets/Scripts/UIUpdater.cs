using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIUpdater : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI acorns;
    private void Start()
    {
        EventManager.AddListener("UpdateAcornUI", UpdateAcorns);
    }

    private void UpdateAcorns(object acornsSize)
    {
        string currentAcorns = ((int)acornsSize).ToString();
        acorns.text = currentAcorns;
    }
}
