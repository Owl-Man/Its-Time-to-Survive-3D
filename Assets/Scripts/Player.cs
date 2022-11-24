using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Values")]

    [SerializeField] private float speed, reloadAttacking;
    private int _attackCombo = 1;
    private bool _isAttacking;

    public GameObject attackHitBox;

    //[HideInInspector] public WeaponItem weaponScript;
    //private Slot _slotScript;

    public Image attackButtonSprite;
    
    [Header("References")]

    [SerializeField] private Animator animator;
    //[SerializeField] private Joystick joystick;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite;
    //[SerializeField] private InventorySystem inventory;
    [SerializeField] private BoxCollider2D rightHit, leftHit;

    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private static readonly int IsSwordEquip = Animator.StringToHash("isSwordEquip");
    private static readonly int IsBowEquip = Animator.StringToHash("isBowEquip");

    public static Player Instance;

    private void Awake() => Instance = this;

    private void Start() => Time.timeScale = 1f;
}