using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApparelManager : MonoBehaviour
{
    [SerializeField] Image ballsOpen;
    [SerializeField] Image ringsOpen;
    [SerializeField] Canvas ballsItems;
    [SerializeField] Canvas ringsItems;

    public void OnChooseMenu(string menuName)
    {
        if (menuName == "balls")
        {
            ringsItems.gameObject.SetActive(false);
            ballsItems.gameObject.SetActive(true);

            ringsOpen.gameObject.SetActive(false);
            ballsOpen.gameObject.SetActive(true);
        }
        else if (menuName == "rings")
        {
            ringsItems.gameObject.SetActive(true);
            ballsItems.gameObject.SetActive(false);

            ringsOpen.gameObject.SetActive(true);
            ballsOpen.gameObject.SetActive(false);
        }
    }
}
