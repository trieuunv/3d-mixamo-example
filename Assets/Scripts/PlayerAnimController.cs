using UnityEngine;
using UnityEngine.InputSystem;   // ← BẮT BUỘC thêm dòng này

/// <summary>
/// Điều khiển animation nhân vật: Idle, Waving, Pose, Jump
/// Sử dụng Input System mới (Input System Package)
/// </summary>
public class PlayerAnimController : MonoBehaviour
{
    [Header("=== Cài đặt phím ===")]
    [Tooltip("Phím để vẫy tay (giữ để vẫy)")]
    public Key waveKey = Key.W;

    [Tooltip("Phím để tạo dáng (giữ để pose)")]
    public Key poseKey = Key.P;

    [Tooltip("Phím để nhảy (nhấn 1 lần)")]
    public Key jumpKey = Key.Space;

    [Header("=== Debug (chỉ xem, không sửa) ===")]
    [SerializeField] private bool isWaving = false;
    [SerializeField] private bool isPosing = false;

    // Component Animator
    private Animator animator;

    // Hash ID của parameter (tối ưu hiệu năng)
    private readonly int HASH_IS_WAVING = Animator.StringToHash("isWaving");
    private readonly int HASH_IS_POSING = Animator.StringToHash("isPosing");
    private readonly int HASH_JUMP      = Animator.StringToHash("jump");

    void Awake()
    {
        // Lấy component Animator
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError($"[{gameObject.name}] Không tìm thấy component Animator!");
            enabled = false;
            return;
        }

        if (animator.avatar == null)
        {
            Debug.LogWarning($"[{gameObject.name}] Animator chưa có Avatar.");
        }

        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError($"[{gameObject.name}] Animator chưa có Controller!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Kiểm tra có bàn phím kết nối không
        if (Keyboard.current == null) return;

        HandleWaving();
        HandlePosing();
        HandleJump();
    }

    /// <summary>
    /// Xử lý vẫy tay - giữ phím để vẫy liên tục
    /// </summary>
    void HandleWaving()
    {
        // Nhấn xuống → bắt đầu vẫy
        if (Keyboard.current[waveKey].wasPressedThisFrame)
        {
            // Nếu đang pose thì tắt pose trước
            if (isPosing)
            {
                isPosing = false;
                animator.SetBool(HASH_IS_POSING, false);
            }

            isWaving = true;
            animator.SetBool(HASH_IS_WAVING, true);
        }

        // Nhả phím → dừng vẫy
        if (Keyboard.current[waveKey].wasReleasedThisFrame)
        {
            isWaving = false;
            animator.SetBool(HASH_IS_WAVING, false);
        }
    }

    /// <summary>
    /// Xử lý tạo dáng - giữ phím để giữ pose
    /// </summary>
    void HandlePosing()
    {
        // Nhấn xuống → bắt đầu pose
        if (Keyboard.current[poseKey].wasPressedThisFrame)
        {
            // Nếu đang vẫy thì tắt vẫy trước
            if (isWaving)
            {
                isWaving = false;
                animator.SetBool(HASH_IS_WAVING, false);
            }

            isPosing = true;
            animator.SetBool(HASH_IS_POSING, true);
        }

        // Nhả phím → thoát pose
        if (Keyboard.current[poseKey].wasReleasedThisFrame)
        {
            isPosing = false;
            animator.SetBool(HASH_IS_POSING, false);
        }
    }

    /// <summary>
    /// Xử lý nhảy - nhấn 1 lần
    /// </summary>
    void HandleJump()
    {
        if (Keyboard.current[jumpKey].wasPressedThisFrame)
        {
            // Tắt các trạng thái khác trước khi nhảy
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

            // Kích hoạt Jump
            animator.SetTrigger(HASH_JUMP);
        }
    }

    // ============ HÀM PUBLIC ============

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