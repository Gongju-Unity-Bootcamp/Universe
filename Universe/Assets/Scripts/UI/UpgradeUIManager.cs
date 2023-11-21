using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UpgradeUIManager : MonoBehaviour
{
    public GameObject _upgradeUI;


    public void SetActiveUpgrade()
    {
        Instantiate(_upgradeUI);
        _upgradeUI.GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
