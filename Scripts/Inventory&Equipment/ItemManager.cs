using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{

    public Equipment[] currentEquipment;
    [SerializeField] PlayerInventory inventory;
    [SerializeField] Transform leftHandTransform;
    [SerializeField] Transform rightHandTransform;
    string handItem = "HandItem";
    [SerializeField] Fighter fighter;
    [SerializeField] StatsModifier playerStats;
    [SerializeField] FixedButton unEquipButton;
    [SerializeField] Equipment defaultEquipment;
    private void Awake()
    {
        fighter = GetComponent<Fighter>();
        playerStats = GetComponent<StatsModifier>();
        inventory = GetComponent<PlayerInventory>();
    }
    void Start()
    {
        
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        Equip(defaultEquipment);
    }
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipmentSlot;
        Equipment oldItem = null;
        if (currentEquipment[slotIndex] != null)
        {

            oldItem = currentEquipment[slotIndex];
            playerStats.UpdateHealth(oldItem.healthModifier, newItem.healthModifier, newItem.isPercent, false);
            playerStats.UpdateSpeed(oldItem.speedModifier, newItem.speedModifier, 0f);
            if (oldItem.GetDisplayName() != "Default")
            { inventory.AddToFirstEmptySlot(oldItem, 1); }
        }
        currentEquipment[slotIndex] = newItem;
        EquipItem(newItem);
        playerStats.UpdateStrength(newItem.damageModifier, newItem.isPercent, 0f);
        playerStats.UpdateArmor(newItem.armorModifier, newItem.isPercent, 0f);
    }

    public void Use(float modifier, bool isPercent, ItemType itemType, float lastingTime)
    {
        switch ((int)itemType)
        {
            case 0:
                playerStats.UpdateHealth(0, modifier, isPercent, false);
                break;
            case 1:
                playerStats.UpdateStrength(modifier, isPercent, lastingTime);
                break;
            case 2:
                playerStats.UpdateArmor(modifier, isPercent, lastingTime);
                break;
            case 3:
                playerStats.UpdateSpeed(0, modifier, lastingTime);
                break;
        }

    }

    void EquipItem(Equipment equipment)
    {
        switch (equipment.equipmentSlot)
        {
            case EquipmentSlot.hands:
                DestroyOldWeapon(rightHandTransform, leftHandTransform);
                GetComponent<Fighter>().SetAttackButtonImages(equipment.attackButtons);
                if (equipment.equipedPrefab1 != null && equipment.equipedPrefab2 != null)
                {
                    GameObject leftHand = Instantiate(equipment.equipedPrefab1, leftHandTransform);
                    GameObject rightHand = Instantiate(equipment.equipedPrefab2, rightHandTransform);
                    leftHand.name = handItem;
                    rightHand.name = handItem;
                }
                else
                {
                    Transform handTransform = equipment.isRightHanded ? rightHandTransform : leftHandTransform;
                    GameObject hand = Instantiate(equipment.equipedPrefab1, handTransform);
                    hand.name = handItem;
                }
                break;
        }
        if (currentEquipment[2].overrideController != null && currentEquipment[2] != null)
        {
            fighter.playerAnimator.runtimeAnimatorController = currentEquipment[2].overrideController;
        }
    }
    void DestroyOldWeapon(Transform rightHand, Transform leftHand)
    {
        Transform oldWeapon1 = rightHand.Find(handItem);
        Transform oldWeapon2 = leftHand.Find(handItem);
        if (oldWeapon1 == null && oldWeapon2 == null) return;
        if (oldWeapon1 != null)
        {
            Destroy(oldWeapon1.gameObject);
        }
        if (oldWeapon2 != null)
        {
            Destroy(oldWeapon2.gameObject);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown("u") || unEquipButton.Pressed)
        {
            UnEquip(2);
        }
    }

    void UnEquip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equip(defaultEquipment);
        }
    }
}
