using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorItem : MonoBehaviour
{
    [SerializeField, Tooltip ("������������ �������")]
    private Item _itemCraft ;

    [SerializeField, Tooltip("����� ��� ������������� ��������")]
    private Transform[] _craftPlace;

    [SerializeField, Tooltip("����������� �������(������)")]
    private GameObject _itemPrefab;
    [SerializeField, Tooltip("��������� ��������")]
    private List<GameObject> _newItem;
    [SerializeField, Tooltip("����� ����������(������) ��������")]
    private GameObject _zoneCraft;
    [SerializeField, Tooltip("����� ����� ��������� ��� �������� ����� ���������")]
    private GameObject _zoneStorage;

    [SerializeField, Tooltip("�������� ����������� ���������")]
    private int _maxItem;
    [SerializeField, Tooltip("����� ���������� ��������")]
    private float _timeCraft;


    [SerializeField,  Tooltip("��������� ������� ")]
    private bool _workGenerator = false;

    private void OnEnable()
    {
        WorkerProduction.onTakeItem += DelItem;
    }

    private void OnDisable()
    {
        WorkerProduction.onTakeItem -= DelItem;

    }

    void Start()
    {
        Transform _placeS = transform.GetChild(0); 
        if (_placeS != null) /// ��������� ����� ��� ����������� ��������
        {
            int PlaceCount = _placeS.childCount;
            _craftPlace = new Transform[PlaceCount];
        }
        else // ���� ��� ����� ��� �������� ������� ������
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < _craftPlace.Length; i++)
        {
            _craftPlace[i] = transform.GetChild(0).GetChild(i);
        }

        if (_zoneCraft == null)
        {
            _workGenerator = true;
            StartCoroutine(CraftItem(_timeCraft));
        }
        

    }

    private void Update()
    {
        if(_newItem.Count < _maxItem && _zoneCraft == null && _workGenerator == false)
        {
            _workGenerator = true;
            StartCoroutine(CraftItem(_timeCraft));
        }

    }

    /// <summary>
    /// ����� �� �������� ��������
    /// </summary>
    /// <returns></returns>
    public float TimerCraft()
    {
        if (_newItem.Count < _maxItem)
        { return _timeCraft; }
        else
        {return 0;}
    }    

    /// <summary>
    /// �������� ���� ������ ��� �������� ����� ���������
    /// </summary>
    /// <returns></returns>
    public GameObject CheckStorage()
    {
        if (_zoneStorage != null)
        {
            return _zoneStorage;
        }
        return null;
    }

    /// <summary>
    /// �������� �������
    /// </summary>
    public void CraftItem()
    {
        int indexPlace = _newItem.Count ;
        int Row = _newItem.Count/ _craftPlace.Length;
        indexPlace = indexPlace - Row * _craftPlace.Length;

        /// �����(position) ��� ������ Item
        Vector3 _NewPosiItem = _craftPlace[indexPlace].position;
        _NewPosiItem.y = _NewPosiItem.y + 0.2f * Row;

        // �������� ������ Item
        GameObject NewItem = Instantiate(_itemPrefab, transform.GetChild(0).position, Quaternion.identity, transform.GetChild(1));
        NewItem.tag = "Item";
        NewItem.GetComponent<OptionsItem>().ScriptableObjItem = _itemCraft;

        NewItem.transform.DOJump(_NewPosiItem, 1f, 1, 0.5f);
        NewItem.name = NewItem.name + indexPlace;
        _newItem.Add(NewItem);
    }

    private IEnumerator CraftItem (float _time)
    {
        var indexPlace = 0;

        while (_newItem.Count < _maxItem)
        {
            CraftItem();

            if (indexPlace < _craftPlace.Length - 1)
            { indexPlace++; }
            else
            { indexPlace = 0; }

            // ���������
            if(_time !=0)
            yield return new WaitForSecondsRealtime(_time);
        }

        _workGenerator = false;

    }

    private void DelItem(GameObject Item)
    {
        if (_newItem.Contains(Item))
        {
            _newItem.Remove(Item);
        }
    }



}
