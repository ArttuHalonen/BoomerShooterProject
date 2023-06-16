using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text ammoText;
    public EquipmentManager gunInfo;

    private void Start()
    {
        ammoText.text = gunInfo.gunCurrentAmmo.ToString();
    }
}
