using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneStorage : MonoBehaviour
{
    [Tooltip("Спикок обьектов в зоне и их позиция")]
    Dictionary<Transform, Vector3> _worker = new Dictionary<Transform, Vector3>();

    [SerializeField, Tooltip("Необходимые предметы")]
    private Item _necessaryItem;

    [SerializeField, Tooltip("В наличие предметов")]
    private List<Transform> _stockItem;
    public List<Transform> StockItem  { get; private set; }

    private bool _takeItem = false;

    private void Start()
    {
        if (_necessaryItem == null)
        {
            Destroy(gameObject);
        }
        StockItem = new List<Transform>();
    }

    private void Update()
    {
        _stockItem = StockItem;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<WorkerProduction>() != null)
        {
            _worker.Add(other.transform, other.transform.position);
        }

    }

    private void OnTriggerStay(Collider other)
    {

        if (_worker.ContainsKey(other.transform) && other.transform.GetComponent<WorkerProduction>() != null)
        {
            if (_worker[other.transform] == other.transform.position && _takeItem == false)
            {
                StartCoroutine(addItemWithDelay(.4f, other.transform));
                _takeItem = true;
            }
            else
            {
                _worker[other.transform] = other.transform.position;
            }
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        _worker.Remove(other.transform);
    }

    private IEnumerator addItemWithDelay(float _time, Transform ownerItem)
    {
        yield return new WaitForSeconds(_time);
        addItem(ownerItem);
    }

    private void addItem(Transform ownerItem)
    {
        var _classWorker = ownerItem.GetComponent<WorkerProduction>();

       
        if (_classWorker.CheckItem(_necessaryItem) == true )
        {
            var _newItem = _classWorker.GiveItem(_necessaryItem);
            if (_newItem != null)
            {
                StockItem.Add(_newItem);
                _newItem.GetComponent<Collider>().enabled = false;
                _newItem.SetParent(transform);
                Vector3 _newItemPosition = (transform.position);
                _newItemPosition.y = transform.position.y + 1.1f * _newItem.localScale.y;
                _newItem.position = _newItemPosition;
            }
        }
        _takeItem = false;
    }

    public void AddNewCraftItem()
    {
        var _delObj = StockItem[StockItem.Count - 1];
        StockItem.RemoveAt(StockItem.Count-1);
        Destroy(_delObj.gameObject);
    }

}
