using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    private TextMeshProUGUI _fpsText;
    [SerializeField] private float _hudRefreshRate = 1f;

    private float _timer;

    void Awake()
    {
        _fpsText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _fpsText.text = "FPS: " + fps;
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
    }
}
