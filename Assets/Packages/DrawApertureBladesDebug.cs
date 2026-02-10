using UnityEngine;

public class DrawApertureBladesDebug : MonoBehaviour
{
    private DrawApertureBlades _drawApertureBlades;

    private void Start()
    {
        _drawApertureBlades = GetComponent<DrawApertureBlades>();
    }

    private void Update()
    {
        _drawApertureBlades.Draw();
    }

    private void OnGUI()
    {
        if(_drawApertureBlades == null) return;

        GUILayout.Label(_drawApertureBlades.ApertureBladesTexture);
    }
}
