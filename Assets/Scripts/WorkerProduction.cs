using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorkerProduction : MonoBehaviour
{
    private Vector3 _positionWorker;

    [SerializeField, Tooltip("Максимальный размер инвернаря")]
    private int _maxItemInventory;

    [SerializeField, Tooltip("список предметов у персонажа")]
    private List<Transform> _inventory = new List<Transform>();

    public static Action<GameObject> onTakeItem;

    private void Start()
    {
        _positionWorker = transform.position;
    }

    void Update()
    {
        if (transform.position != _positionWorker)
        {
            _positionWorker = transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_positionWorker == transform.position)
        {
            if (other.CompareTag("Item") && _inventory.Count < _maxItemInventory)
            {
                onTakeItem?.Invoke(other.gameObject);
                other.transform.SetParent(transform);
                _inventory.Add(other.transform);

                Vector3 _NewPosiItem = new Vector3(0, 0 + (_inventory.Count - 1) * other.transform.localScale.y, 1 + other.transform.localScale.z / 2);

                Vector3 localForward = transform.forward;
                // Получаем мировое направление объекта
                Vector3 worldForward = transform.TransformDirection(Vector3.forward);

                _NewPosiItem = _NewPosiItem + transform.position + worldForward;

                other.transform.DOMove(_NewPosiItem, 0.5f);
                other.transform.DORotate(Vector3.zero, 0.5f);

            }
        }
    }

    /// <summary>
    /// Проверка наличия предмета
    /// </summary>
    /// <param name="_item"></param>
    /// <returns></returns>
    public bool CheckItem(Item _item) 
    {
        foreach (var item in _inventory)
        {
           if(item.GetComponent<OptionsItem>().ScriptableObjItem == _item)
            {
                return true;
            }

        }
        return false;
    } 

        /// <summary>
        /// Отдать предмет
        /// </summary>
        /// <param name="_transItem"></param>
    public Transform GiveItem (Item _item)
    {
        if (_inventory.Count > 0)
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].GetComponent<OptionsItem>().ScriptableObjItem == _item)
                {
                    Transform _giveItem = _inventory[i];
                    _inventory.RemoveAt(i);
                    return _giveItem;
                }
            }
        }
        return null;
    }



}
