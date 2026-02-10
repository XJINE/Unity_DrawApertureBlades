using UnityEngine;

public class DrawApertureBlades: MonoBehaviour
{
    [SerializeField] private ComputeShader computeShader;

    [Range(  64,  512)] public int   textureSize = 128;
    [Range(   3,   10)] public int   bladeNum    = 6;
    [Range(0.0f, 1.0f)] public float radius      = 0.5f;

    private int           _kernelDrawApertureBlades;
    private Vector3Int    _tgsDrawApertureBlades;
    private RenderTexture _apertureBladesTexture;

    public RenderTexture ApertureBladesTexture => _apertureBladesTexture;

    private static class PID
    {
        public static int _ApertureBladeTexture = Shader.PropertyToID(nameof(_ApertureBladeTexture));
        public static int _BladeNum             = Shader.PropertyToID(nameof(_BladeNum));
        public static int _Radius               = Shader.PropertyToID(nameof(_Radius));
    }

    private void Awake()
    {
        _kernelDrawApertureBlades = computeShader.FindKernel("DrawApertureBlades");

        computeShader.GetKernelThreadGroupSizes(_kernelDrawApertureBlades, out var x, out var y, out var z);

        _tgsDrawApertureBlades = new Vector3Int((int)x, (int)y, (int)z);

        _apertureBladesTexture = new RenderTexture(textureSize, textureSize, 0,
                                                   RenderTextureFormat.ARGB32,
                                                   RenderTextureReadWrite.Linear)
        {
            enableRandomWrite = true
        };

        _apertureBladesTexture.Create();
    }

    public void Draw()
    {
        computeShader.SetInt    (PID._BladeNum, bladeNum);
        computeShader.SetFloat  (PID._Radius, radius);
        computeShader.SetTexture(_kernelDrawApertureBlades, PID._ApertureBladeTexture, _apertureBladesTexture);

        computeShader.Dispatch(_kernelDrawApertureBlades,
                               Mathf.CeilToInt((float)textureSize / _tgsDrawApertureBlades.x),
                               Mathf.CeilToInt((float)textureSize / _tgsDrawApertureBlades.y),
                               1);
    }

    private void OnDestroy()
    {
        if (_apertureBladesTexture != null)
        {
            _apertureBladesTexture.Release();
            _apertureBladesTexture = null;
        }
    }
}