using UnityEngine;
using UnityEngine.InputSystem;   // ← BẮT BUỘC thêm dòng này

/// <summary>
/// Điều khiển animation nhân vật: Idle, Waving, Pose, Jump
/// Sử dụng Input System mới (Input System Package)
/// </summary>
public class PlayerAnimController : MonoBehaviour
{
    [Header("Key Setup")]
    [Tooltip("Button to wave (hold to wave)")]
    public Key waveKey = Key.W;

    [Tooltip("Button to pose (hold to pose)")]
    public Key poseKey = Key.P;

    [Tooltip("Button to jump (click once)")]
    public Key jumpKey = Key.Space;

    [Header("Debug")]
    [SerializeField] private bool isWaving = false;
    [SerializeField] private bool isPosing = false;

    private Animator animator;

    private readonly int HASH_IS_WAVING = Animator.StringToHash("isWaving");
    private readonly int HASH_IS_POSING = Animator.StringToHash("isPosing");
    private readonly int HASH_JUMP      = Animator.StringToHash("jump");

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError($"[{gameObject.name}] Can't find Animator component!");
            enabled = false;
            return;
        }

        if (animator.avatar == null)
        {
            Debug.LogWarning($"[{gameObject.name}] Animator no Avatar.");
        }

        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError($"[{gameObject.name}] Animator no Controller!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        HandleWaving();
        HandlePosing();
        HandleJump();
    }

    void HandleWaving()
    {
        if (Keyboard.current[waveKey].wasPressedThisFrame)
        {
            if (isPosing)
            {
                isPosing = false;
                animator.SetBool(HASH_IS_POSING, false);
            }

            isWaving = true;
            animator.SetBool(HASH_IS_WAVING, true);
        }

        if (Keyboard.current[waveKey].wasReleasedThisFrame)
        {
            isWaving = false;
            animator.SetBool(HASH_IS_WAVING, false);
        }
    }

    void HandlePosing()
    {
        if (Keyboard.current[poseKey].wasPressedThisFrame)
        {
            if (isWaving)
            {
                isWaving = false;
                animator.SetBool(HASH_IS_WAVING, false);
            }

            isPosing = true;
            animator.SetBool(HASH_IS_POSING, true);
        }

        if (Keyboard.current[poseKey].wasReleasedThisFrame)
        {
            isPosing = false;
            animator.SetBool(HASH_IS_POSING, false);
        }
    }

    void HandleJump()
    {
        if (Keyboard.current[jumpKey].wasPressedThisFrame)
        {
            if (isWaving)
            {
                isWaving = false;
                animator.SetBool(HASH_IS_WAVING, false);
            }
            if (isPosing)
            {
                isPosing = false;
                animator.SetBool(HASH_IS_POSING, false);
            }

            animator.SetTrigger(HASH_JUMP);
        }
    }

    public void StartWaving()
    {
        isWaving = true;
        animator.SetBool(HASH_IS_WAVING, true);
    }

    public void StopWaving()
    {
        isWaving = false;
        animator.SetBool(HASH_IS_WAVING, false);
    }

    public void StartPosing()
    {
        isPosing = true;
        animator.SetBool(HASH_IS_POSING, true);
    }

    public void StopPosing()
    {
        isPosing = false;
        animator.SetBool(HASH_IS_POSING, false);
    }

    public void DoJump()
    {
        animator.SetTrigger(HASH_JUMP);
    }
}