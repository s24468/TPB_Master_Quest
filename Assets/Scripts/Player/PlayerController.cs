using System.Collections;
using DataPersistance;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    public float moveSpeed = 5f;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;
    public bool isMoving;
    private Vector2 _input;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = Input.GetAxisRaw("Vertical");
            //remove diagonal movement
            if (_input.x != 0)
            {
                _input.y = 0;
            }

            if (_input != Vector2.zero)
            {
                _animator.SetFloat("moveX", _input.x);
                _animator.SetFloat("moveY", _input.y);
                var targetPos = transform.position;
                targetPos.x += _input.x;
                targetPos.y += _input.y;
                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        _animator.SetBool(IsMoving, isMoving);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        CheckForEncounters();
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) is null)
        {
            return;
        }
        if (Random.Range(0, 101) <= 10)
        {
            Debug.Log("Encounter Pokemon");
        }
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) is null;
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}