using UnityEngine;
using System.IO;
using System.Collections;

public class CaptureUIPanel : MonoBehaviour
{
    public RectTransform panelToCapture;

    public void CapturePanel()
    {
        StartCoroutine(CaptureRoutine());
    }

    private IEnumerator CaptureRoutine()
    {
        yield return new WaitForEndOfFrame();

        Vector3[] corners = new Vector3[4];
        panelToCapture.GetWorldCorners(corners);

        float x = corners[0].x;
        float y = corners[0].y;
        float width = corners[2].x - x;
        float height = corners[2].y - y;

        Texture2D tex = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(x, y, width, height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        string path = Application.persistentDataPath + "/panel_capture.png";
        File.WriteAllBytes(path, bytes);

        Debug.Log("Panel guardado en: " + path);
    }
}
