using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneCraft : MonoBehaviour
{

    [Tooltip("������ �������� � ���� � �� �������")]
    Dictionary<Transform, Vector3> _worker = new Dictionary<Transform, Vector3>();

    [SerializeField, Tooltip("����������� ����������� ������ ")]
    private Image _progressbar ;

    [SerializeField, Tooltip("����� ��� �������� ��������")]
    private float _timeCraft;

    [SerializeField, Tooltip("����������� ������ ")]
    private bool _workCraft = false;

    [Tooltip("���� ������ ��������� ��� �������� ����� ���������")]
    private ZoneStorage _zoneStorage;

    private void Start()
    {
        _progressbar.fillAmount = 0;
        _timeCraft = transform.parent.GetComponent<GeneratorItem>().TimerCraft();
        if (transform.parent.GetComponent<GeneratorItem>().CheckStorage() != null)
        {
            _zoneStorage = transform.parent.GetComponent<GeneratorItem>().CheckStorage().GetComponent<ZoneStorage>();
        }
    }

    private void Update()
    {
            CraftJob();
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
        if (other.transform.GetComponent<WorkerProduction>() != null)
        {
            if ( _worker.ContainsKey(other.transform))
            {

                if (_worker[other.transform] == other.transform.position)
                {
                    _workCraft = true;
                }
                else
                {
                    _workCraft = false;
                    _worker[other.transform] = other.transform.position;
                    _progressbar.fillAmount = 0;
                    _timeCraft = transform.parent.GetComponent<GeneratorItem>().TimerCraft();
                }
            }
        }

    }

        private void OnTriggerExit(Collider other)
    {
        _worker.Remove(other.transform);
    }

    /// <summary>
    ///  ��������� ������ �� ������
    /// </summary>
    private void CraftJob()
    {
        if (transform.parent.GetComponent<GeneratorItem>() != null)
        {
            var _parentGeneratorItem = transform.parent.GetComponent<GeneratorItem>();
            if (_workCraft != true)
            { _timeCraft = _parentGeneratorItem.TimerCraft(); }

            if (_zoneStorage == null)
            { TimeCraftJob(); }
            else if(_zoneStorage.StockItem.Count > 0)
            {
                TimeCraftJob();
            }

        }
        else
        {
            Destroy(gameObject);
        }    
    }

    private void TimeCraftJob()
    {
        var _parentGeneratorItem = transform.parent.GetComponent<GeneratorItem>();

        if (_parentGeneratorItem.TimerCraft() != 0 && _workCraft == true)
        {
            if (_timeCraft <= 0)
            {
                if (_zoneStorage != null)
                {
                    _zoneStorage.AddNewCraftItem();
                }

                    _timeCraft = _parentGeneratorItem.TimerCraft();
                _parentGeneratorItem.CraftItem();
                _progressbar.fillAmount = 0;
            }
            else
            {
                _timeCraft -= Time.deltaTime;
                _progressbar.fillAmount = 1 - (_timeCraft / _parentGeneratorItem.TimerCraft());
            }
        }
    }


}
