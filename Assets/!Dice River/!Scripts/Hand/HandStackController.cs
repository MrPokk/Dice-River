using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HandStackController<TData, TView> : MonoBehaviour where TView : MonoBehaviour
{
    private struct Entry { public TData data; public TView prefab; }

    [Header("Settings")]
    [SerializeField] private bool _isVertical;
    [SerializeField] private float _space = 5f;
    [SerializeField] private float _parabola = 2000f;
    [SerializeField] private bool _rotate;

    private HandController<TData, TView> _hand;
    private RectTransform _rect;
    private readonly Stack<Entry> _stack = new();
    private readonly Dictionary<TData, TView> _views = new();

    public int Count => _stack.Count;

    private void Awake() => _rect = GetComponent<RectTransform>();

    public virtual void Initialize(HandController<TData, TView> hand) => _hand = hand;

    public void Add(TData item, TView prefab)
    {
        _stack.Push(new Entry { data = item, prefab = prefab });
        if (prefab != null) _views[item] = Instantiate(prefab, _rect);
        UpdateLayout();
    }

    public bool DrawToHand()
    {
        if (_stack.Count == 0 || _hand == null) return false;

        var entry = _stack.Pop();
        if (_views.Remove(entry.data, out var view)) Destroy(view.gameObject);

        if (!_hand.Add(entry.data, entry.prefab))
        {
            Add(entry.data, entry.prefab);
            return false;
        }

        UpdateLayout();
        return true;
    }

    public void Shuffle()
    {
        var list = _stack.ToList();
        _stack.Clear();
        foreach (var e in list.OrderBy(_ => Random.value)) _stack.Push(e);
        UpdateLayout();
    }

    public void SetContainer(Transform container)
    {
        _rect = container.GetComponent<RectTransform>();
        foreach (var v in _views.Values) v.transform.SetParent(_rect, false);
        UpdateLayout();
    }

    private void UpdateLayout()
    {
        var items = _stack.ToArray();
        System.Array.Reverse(items);
        var offset = (items.Length - 1) / 2f;

        for (int i = 0; i < items.Length; i++)
        {
            if (!_views.TryGetValue(items[i].data, out var view)) continue;

            var rt = view.transform as RectTransform;
            var x = (i - offset) * _space;
            var y = -(x * x) / _parabola;

            rt.anchoredPosition = _isVertical ? new Vector2(y, -x) : new Vector2(x, y);
            rt.localRotation = _rotate ? Quaternion.Euler(0, 0, Mathf.Atan(-2 * x / _parabola) * Mathf.Rad2Deg) : Quaternion.identity;
            rt.SetSiblingIndex(i);
        }
    }
}
