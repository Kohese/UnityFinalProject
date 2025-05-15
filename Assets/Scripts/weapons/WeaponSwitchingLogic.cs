using UnityEngine;
using WeaponStrategyPattern;

public class WeaponSwitchingLogic : MonoBehaviour
{
    private WeaponTiers wepTier;
    private int currentTier = 1;
    public int totalWeapons = 5;
    // public bool switchOutWeapon;
    private void Start()
    {
        gameObject.AddComponent<WeaponTiers>();
        wepTier = GetComponent<WeaponTiers>();
        wepTier.SwitchWeapon(currentTier);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))    // TEST
        {
            NextWeapon();
        }
    }

    public void NextWeapon()
    {   
        if (currentTier < totalWeapons) {
            // wepTier.switchOutWeapon(currentTier);    
            currentTier++;
            wepTier.SwitchWeapon(currentTier);
            Debug.Log($"Advanced to Tier {currentTier}");
        } else {
            Debug.Log("Limit reached");
        }
    }
}
