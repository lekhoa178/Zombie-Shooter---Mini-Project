using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource), typeof(CanvasGroup))]
[DisallowMultipleComponent]
public class Page : MonoBehaviour
{
    private AudioSource AudioSource;
    private CanvasGroup CanvasGroup;
    private RectTransform RectTransform;

    [SerializeField] private float _AnimationSpeed = 1f;
    public bool ExitOnNewPagePush = false;
    public bool EnableOnCancel = true;
    [SerializeField] private AudioClip _EntryClip;
    [SerializeField] private AudioClip _ExitClip;
    [SerializeField] private EntryMode _EntryMode = EntryMode.SLIDE;
    [SerializeField] private Direction _EntryDirection = Direction.LEFT;
    [SerializeField] private EntryMode _ExitMode = EntryMode.SLIDE;
    [SerializeField] private Direction _ExitDirection = Direction.LEFT;

    private Coroutine _AnimationCoroutine;
    private Coroutine _AudioCoroutine;

    [SerializeField] private UnityEvent _OnPrePush;
    [SerializeField] private UnityEvent _OnPostPush;
    [SerializeField] private UnityEvent _OnPrePop;
    [SerializeField] private UnityEvent _OnPostPop;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();
        AudioSource = GetComponent<AudioSource>();

        AudioSource.playOnAwake = false;
        AudioSource.loop = false;
        AudioSource.spatialBlend = 0;
        AudioSource.enabled = false;
    }

    public void Enter(bool playAudio)
    {
        gameObject.SetActive(true);
        _OnPrePush?.Invoke();

        switch (_EntryMode)
        {
            case EntryMode.SLIDE:
                SlideIn(playAudio);
                break;
            case EntryMode.ZOOM:
                ZoomIn(playAudio);
                break;
            case EntryMode.FADE:
                FadeIn(playAudio);
                break;
            case EntryMode.NONE:
                _OnPostPush?.Invoke();
                break;
        }
    }

    public void Exit(bool playAudio, bool onPop = true)
    {
        if (onPop)
            _OnPrePop?.Invoke();

        switch (_ExitMode)
        {
            case EntryMode.SLIDE:
                SlideOut(playAudio, onPop);
                break;
            case EntryMode.ZOOM:
                ZoomOut(playAudio, onPop);
                break;
            case EntryMode.FADE:
                FadeOut(playAudio, onPop);
                break;
            case EntryMode.NONE:
                if (onPop)
                    _OnPostPop?.Invoke();
                gameObject.SetActive(false);
                break;
        }
    }

    private void SlideIn(bool playAudio)
    {
        if (_AnimationCoroutine != null)
        {
            StopCoroutine(_AnimationCoroutine);
        }

        _AnimationCoroutine = StartCoroutine(AnimationHelper.SlideIn(RectTransform, _EntryDirection, _AnimationSpeed, _OnPostPush));

        PlayEntryClip(playAudio);
    }

    private void SlideOut(bool playAudio, bool onPop = true)
    {
        if (_AnimationCoroutine != null)
        {
            StopCoroutine(_AnimationCoroutine);
        }

        if (onPop)
            _AnimationCoroutine = StartCoroutine(AnimationHelper.SlideOut(RectTransform, _ExitDirection, _AnimationSpeed, _OnPostPop));
        else
            _AnimationCoroutine = StartCoroutine(AnimationHelper.SlideOut(RectTransform, _ExitDirection, _AnimationSpeed, null));

        PlayExitClip(playAudio);
    }

    private void ZoomIn(bool playAudio)
    {
        if (_AnimationCoroutine != null)
        {
            StopCoroutine(_AnimationCoroutine);
        }

        _AnimationCoroutine = StartCoroutine(AnimationHelper.ZoomIn(RectTransform, _AnimationSpeed, _OnPostPush));

        PlayEntryClip(playAudio);
    }

    private void ZoomOut(bool playAudio, bool onPop = true)
    {
        if (_AnimationCoroutine != null)
        {
            StopCoroutine(_AnimationCoroutine);
        }

        if (onPop)
            _AnimationCoroutine = StartCoroutine(AnimationHelper.ZoomOut(RectTransform, _AnimationSpeed, _OnPostPop));
        else
            _AnimationCoroutine = StartCoroutine(AnimationHelper.ZoomOut(RectTransform, _AnimationSpeed, null));


        PlayExitClip(playAudio);
    }

    private void FadeIn(bool playAudio)
    {
        if (_AnimationCoroutine != null)
        {
            StopCoroutine(_AnimationCoroutine);
        }

        _AnimationCoroutine = StartCoroutine(AnimationHelper.FadeIn(CanvasGroup, _AnimationSpeed, _OnPostPush));

        PlayEntryClip(playAudio);
    }

    private void FadeOut(bool playAudio, bool onPop = true)
    {
        if (_AnimationCoroutine != null)
        {
            StopCoroutine(_AnimationCoroutine);
        }

        if (onPop)
            _AnimationCoroutine = StartCoroutine(AnimationHelper.FadeOut(CanvasGroup, _AnimationSpeed, _OnPostPop));
        else
            _AnimationCoroutine = StartCoroutine(AnimationHelper.FadeOut(CanvasGroup, _AnimationSpeed, null));

        PlayExitClip(playAudio);
    }

    private void PlayEntryClip(bool playAudio)
    {
        if (playAudio && _EntryClip != null && AudioSource != null)
        {
            if (_AudioCoroutine != null)
                StopCoroutine(_AudioCoroutine);

            _AudioCoroutine = StartCoroutine(PlayClip(_EntryClip));
        }

    }

    private void PlayExitClip(bool playAudio)
    {
        if (playAudio && _ExitClip != null && AudioSource != null)
        {
            if (_AudioCoroutine != null)
                StopCoroutine(_AudioCoroutine);

            _AudioCoroutine = StartCoroutine(PlayClip(_ExitClip));

        }
    }

    private IEnumerator PlayClip(AudioClip clip)
    {
        AudioSource.enabled = true;
        WaitForSeconds wait = new WaitForSeconds(clip.length);
        AudioSource.PlayOneShot(clip);
        yield return wait;
        AudioSource.enabled = false;
    }
}

public enum EntryMode
{
    NONE,
    SLIDE,
    ZOOM,
    FADE
}

public enum Direction
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}