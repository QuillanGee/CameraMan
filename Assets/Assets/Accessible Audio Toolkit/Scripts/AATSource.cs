using UnityEngine;
using UnityEngine.UI;

public class AATSource : MonoBehaviour
{
    public AATManager manager;

    Image icon;
    Image arrow;
    AudioSource source;
    GameObject marker;

    Color visible;
    Color invisible;
    Vector3 originalScale;

    private void Start()
    {
        visible = new Color(1, 1, 1, 1);
        invisible = new Color(0, 0, 0, 0);
        source = gameObject.GetComponent<AudioSource>();
        manager.AddSource(this);
    }

    private void OnDestroy()
    {
        Destroy(marker);
        manager.RemoveSource(this);
    }

    // Show/Hide compass marker arrow, flipped if needed
    public void ShowArrow(bool show, bool flipped)
    {
        if (show)
            arrow.color = visible;
        else
            arrow.color = invisible;

        arrow.transform.localScale = originalScale;
        if (flipped)
            arrow.transform.localScale = -originalScale;
    }

    // GETTERS AND SETTERS
    public void SetMarker(GameObject m) { marker = m; }

    public Vector2 position {
        get { return new Vector2(transform.position.x, transform.position.z); }
    }

    public Vector3 positionXYZ
    {
        get { return transform.position; }
    }

    public AudioSource GetAudioSource() { return source; }

    public Image GetIcon() { return icon; }

    public void SetIcon(Image im) { icon = im; }

    public Image GetArrow() { return arrow; }

    public void SetArrow(Image arr) { 
        arrow = arr;
        originalScale = arrow.transform.localScale;
    }
}
