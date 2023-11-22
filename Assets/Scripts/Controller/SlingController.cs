using System;
using UnityEngine;

public class SlingController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private TrayectoryHandler trayectoryHandler = null;

    [SerializeField] private Transform elasticBand = null;
    [SerializeField] private Transform center = null;
    [SerializeField] private float bounceOffset = 0;

    [Header("X movement")]
    [SerializeField] private float minScale = 0.01f;
    [SerializeField] private float maxScale = 0.1f;
    [SerializeField] private float scaleOffset = 100f;
    [SerializeField] private float scaleLerperSpeed = 1.0f;

    [Header("Y movement")]
    [SerializeField] private float minEuler = -150f;
    [SerializeField] private float maxEuler = -210f;
    [SerializeField] private float eulerOffset = 100f;
    [SerializeField] private float eulerLerperSpeed = 1.0f;

    [Header("Trayectory")]
    [SerializeField] private float yTrayectoryOffset = 0.0f;
    [SerializeField] private float depthTrayectoryOffset = 0.0f;

    [Header("Shoot")]
    [SerializeField] private float minShootingDepth = 0.3f;
    [SerializeField] private float forceOffset = 1.0f;
    #endregion

    #region PRIVATE_FIELDS
    private BallEntity ballPrefab = null;

    private Vector3 trayectoryDepthValue = Vector3.zero;
    private Vector3 trayectoryHeightValue = Vector3.zero;

    private Vector3 tapInitialPosition = Vector3.zero;

    private Vector3 lerpToScale = Vector3.zero;
    private Vector3 initialScale = Vector3.zero;

    private Vector3 lerpToEuler = Vector3.one;
    private Vector3 initialRotation = Vector3.zero;

    private bool onDespawnTransition = false;

    private Action onSpawned = null;
    private float yOffset = 0.0f;
    #endregion

    #region PROPERTIES
    public bool OnDespawnTransition { get => onDespawnTransition; set => onDespawnTransition = value; }
    public Action OnSpawned { get => onSpawned; set => onSpawned = value; }
    public float MaxDepth { get => maxScale; }
    public float MinDepth { get => minScale; }
    public float CurrDepth { get => yOffset; }
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        lerpToScale = elasticBand.localScale;
        initialScale = elasticBand.localScale;

        lerpToEuler = transform.eulerAngles;
        initialRotation = transform.eulerAngles;
    }
    private void Update()
    {
        CheckInput();

        LerpBandScale(lerpToScale);
        LerpBandEuler(lerpToEuler);
        if (elasticBand.localScale.x > minScale)
        {
            CalculateTrayectory();
        }
    }
    #endregion

    #region PRIVATE_METHODS
    private void CalculateTrayectory()
    {
        trayectoryDepthValue = new Vector3(transform.position.x - (transform.forward.x * elasticBand.localScale.z * depthTrayectoryOffset), center.transform.position.y,
            transform.position.z * (-transform.forward.z * elasticBand.localScale.z * depthTrayectoryOffset));

        trayectoryDepthValue = new Vector3(trayectoryDepthValue.x, Mathf.Clamp(trayectoryDepthValue.y, center.transform.position.y, float.MaxValue),
            Mathf.Clamp(trayectoryDepthValue.z, center.transform.position.z, float.MaxValue));

        trayectoryHeightValue = trayectoryDepthValue - center.position;

        trayectoryHeightValue /= 2;
        trayectoryHeightValue += center.position;
        trayectoryHeightValue.y += yTrayectoryOffset * elasticBand.localScale.z;

        if (trayectoryHandler != null)
            trayectoryHandler.SetMatrix(center.position, trayectoryHeightValue, trayectoryDepthValue);
    }

    private void CalculateBandMovement(Vector3 tapActualPosition)
    {
        yOffset = tapInitialPosition.y - tapActualPosition.y;
        float xOffset = tapInitialPosition.x - tapActualPosition.x;

        lerpToScale = elasticBand.localScale;

        lerpToEuler = transform.eulerAngles;

        yOffset = yOffset * scaleOffset / maxScale;
        yOffset = Mathf.Clamp(yOffset, minScale, maxScale);

        xOffset /= eulerOffset;
        float normalYEuler = (maxEuler - minEuler) / 2 + minEuler;
        xOffset += normalYEuler;
        xOffset = Mathf.Clamp(xOffset, minEuler, maxEuler);

        lerpToEuler.y = xOffset;
        lerpToScale.z = yOffset;
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            tapInitialPosition = Input.mousePosition;
            trayectoryHandler.ToggleTrayectory(true);
        }

        if (Input.GetMouseButton(0))
        {
            CalculateBandMovement(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (CurrDepth > minShootingDepth)
            {
                ShootBall();
            }

            lerpToScale = initialScale;
            lerpToEuler = initialRotation;
            trayectoryHandler.ToggleTrayectory(false);
        }
    }

    private void LerpBandScale(Vector3 endScale)
    {
        elasticBand.localScale = Vector3.Lerp(elasticBand.localScale, endScale, scaleLerperSpeed);
    }
    private void LerpBandEuler(Vector3 endEuler)
    {
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, endEuler, eulerLerperSpeed);
    }

    private void ShootBall()
    {
        GameObject ball = Instantiate(ballPrefab.gameObject, center.position, Quaternion.identity);
        Vector3 force = new Vector3(trayectoryDepthValue.x, trayectoryHeightValue.y, trayectoryDepthValue.z);
        ball.transform.LookAt(force);
        ball.GetComponent<Rigidbody>().AddForce(force * forceOffset);
        ball.GetComponent<Animation>().PlayQueued("Roll", QueueMode.PlayNow);
        for (int i = 0; i < 10; i++)
        {
            ball.GetComponent<Animation>().PlayQueued("Roll");
        }
        Destroy(ball, 5);
    }
    #endregion

    #region PUBLIC_METHODS
    public void InitGameplay(BallEntity ballEntity)
    {
        this.ballPrefab = ballEntity;
    }

    public void OnSpawn()
    {
        onSpawned?.Invoke();

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("despawn", false);
        }
    }

    public void SetDespawnAnim()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("despawn", true);
        }
    }
    #endregion
}
