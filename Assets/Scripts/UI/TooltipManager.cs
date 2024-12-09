using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;
    [SerializeField] private TextMeshProUGUI _textComponent;
    private bool _isVisible;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        Cursor.visible = true;
        _isVisible = false;
        gameObject.SetActive(false);
    }
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
    public void SetAndShowTooltip(string message)
    {
            _isVisible = true;
            gameObject.SetActive(true);
            _textComponent.text = message;
    }
    public void HideToolTip()
    {
            _isVisible = false;
            gameObject.SetActive(false);
            _textComponent.text = string.Empty;
    }
}
