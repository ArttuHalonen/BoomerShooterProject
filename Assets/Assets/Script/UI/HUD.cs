using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text ammoText;
    public EquipmentManager gunInfo;

    private void Start()
    {

    }
    private void Update()
    {
        ammoText.text = gunInfo.gunCurrentAmmo.ToString();
    }
}
