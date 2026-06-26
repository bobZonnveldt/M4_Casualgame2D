using TMPro;
using UnityEngine;
public class shoot : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject prefabIndicator0;
    [SerializeField] private GameObject prefabIndicator1;
    [SerializeField] private GameObject prefabIndicator2;

    [SerializeField] private float forceBuild = 20f;
    [SerializeField] private float maximumHoldTime = 5f;
    [SerializeField] private float lineSpeed = 10f;
    public  TextMeshProUGUI PointsText;
    private LineRenderer _line;
    private bool _lineActive = false;

    private float _pressTimer = 0f;
    private float _launchForce = 0f;
    private int _selectedPrefabIndex = -1;
    
    private int totalScore = 0;
    public static shoot Instance { get; private set; }
    

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        _line = GetComponent<LineRenderer>();
        _line.SetPosition(1, Vector3.zero);
        ResetPrefabIndicators();
        UpdateScoreUI();
    }

    private void Update()
    {
        HandleShot();
      
       
    }

    private void HandleShot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _pressTimer = 0;
            _lineActive = true;

            if (prefabs.Length > 0)
            {
                _selectedPrefabIndex = Random.Range(0, prefabs.Length);
                SetPrefabIndicator(_selectedPrefabIndex, true);
            }
            else
            {
                _selectedPrefabIndex = -1;
                ResetPrefabIndicators();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _lineActive = false;
            _line.SetPosition(1, Vector3.zero);
            
            _launchForce = _pressTimer * forceBuild;

            if (prefabs.Length == 0 || _selectedPrefabIndex < 0 || _selectedPrefabIndex >= prefabs.Length)
            {
                return;
            }

            GameObject ball = Instantiate(prefabs[_selectedPrefabIndex], transform.parent);
            ball.transform.rotation = transform.rotation;
            ball.transform.position = transform.position;
            ball.GetComponent<Rigidbody2D>().AddForce(ball.transform.right * _launchForce, ForceMode2D.Impulse);

            if (OrbSpawner.Instance != null)
            {
                OrbSpawner.Instance.HandleShotFired();
            }
        }

        if (_lineActive)
        {
            _line.SetPosition(1, Vector3.right * _pressTimer * lineSpeed);
        }

        if (_pressTimer < maximumHoldTime)
        {
            _pressTimer += Time.deltaTime;
        }
    }

    private void SetPrefabIndicator(int index, bool isActive)
    {
        ResetPrefabIndicators();

        if (index == 0 && prefabIndicator0 != null)
        {
            prefabIndicator0.SetActive(isActive);
        }
        else if (index == 1 && prefabIndicator1 != null)
        {
            prefabIndicator1.SetActive(isActive);
        }
        else if (index == 2 && prefabIndicator2 != null)
        {
            prefabIndicator2.SetActive(isActive);
        }
    }

    private void ResetPrefabIndicators()
    {
        if (prefabIndicator0 != null)
        {
            prefabIndicator0.SetActive(false);
        }

        if (prefabIndicator1 != null)
        {
            prefabIndicator1.SetActive(false);
        }

        if (prefabIndicator2 != null)
        {
            prefabIndicator2.SetActive(false);
        }
    }
    
    public void AddScore(int points)
    {
        totalScore += points;
        UpdateScoreUI();
    }
    
    private void UpdateScoreUI()
    {
        if (PointsText != null)
        {
            PointsText.text = "Points: " + totalScore;
        }
    }
}