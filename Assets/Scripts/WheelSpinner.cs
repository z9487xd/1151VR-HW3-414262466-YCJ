using UnityEngine;

public class WheelSpinner : MonoBehaviour
{
    public float accelRate = 700f;
    public float decelRate = 220f;
    public float minTopSpeed = 700f;
    public float maxTopSpeed = 1200f;
    public string[] results = { "大吉", "吉", "小吉", "末吉", "平", "小凶", "凶", "大凶" };

    public AudioSource audioSource; // 負責播放聲音的元件
    public AudioClip resultSound;   // 停下來時要播放的音效檔案

    enum Phase { Idle, Accel, Decel }
    Phase phase = Phase.Idle;
    float speed = 0;
    float topSpeed;

    void Start()
    {
        Application.targetFrameRate = 60;

        // 自動防呆：如果忘記在 Inspector 拖入 AudioSource，就自動從身上抓取
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (phase == Phase.Idle && Input.GetMouseButtonDown(0))
        {
            topSpeed = Random.Range(minTopSpeed, maxTopSpeed);
            speed = 0f;
            phase = Phase.Accel;
        }

        if (phase == Phase.Accel)
        {
            speed += accelRate * Time.deltaTime;
            if (speed >= topSpeed)
            {
                speed = topSpeed;
                phase = Phase.Decel;
            }
        }
        else if (phase == Phase.Decel)
        {
            speed -= decelRate * Time.deltaTime;
            if (speed <= 0)
            {
                speed = 0;
                phase = Phase.Idle;
                GetResult();
            }
        }

        transform.Rotate(0, 0, -speed * Time.deltaTime);
    }

    void GetResult()
    {
        float currentAngle = transform.eulerAngles.z;
        int index = Mathf.FloorToInt(currentAngle / 45f);
        Debug.Log("result: " + results[index]);

        // ===== 新增：在最後播放音效 =====
        if (audioSource != null && resultSound != null)
        {
            audioSource.PlayOneShot(resultSound);
        }
    }
}
