using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class HandController<TData, TView> : MonoBehaviour where TView : MonoBehaviour
{
    private readonly List<TData> _dataItems = new();
    private readonly Dictionary<TData, TView> _viewMap = new();
    private RectTransform _containerRect;

    public IReadOnlyCollection<TData> Items => _dataItems;
    [ReadOnly] public HandStackController<TData, TView> handStackController;
    public event Action OnChanged;

    public virtual void Initialize(HandStackController<TData, TView> handStackController)
    {
        this.handStackController = handStackController;
    }

    private void Awake()
    {
        if (_containerRect == null) _containerRect = GetComponent<RectTransform>();
    }

    public void SetContainer(Transform container)
    {
        var newRect = container.GetComponent<RectTransform>();
        if (_containerRect == newRect) return;
        _containerRect = newRect;

        foreach (var view in _viewMap.Values)
        {
            view?.transform.SetParent(_containerRect, false);
        }
        OnChanged?.Invoke();
        OnChangedInternal();
    }

    public virtual bool Add(TData data, TView viewPrefab)
    {
        if (data == null || _viewMap.ContainsKey(data)) return false;

        var startWorldPos = transform.position;
        if (handStackController != null)
        {
            var stackView = handStackController.GetView(data);
            if (stackView != null) startWorldPos = stackView.transform.position;
        }

        var viewInstance = Instantiate(viewPrefab, _containerRect);

        viewInstance.transform.position = startWorldPos;

        _dataItems.Add(data);
        _viewMap.Add(data, viewInstance);

        AnimateEntry(viewInstance);
        OnChanged?.Invoke();
        OnChangedInternal();
        return true;
    }

    protected virtual void AnimateEntry(TView view) { }
    protected virtual void OnChangedInternal() { }

    public virtual bool ExtractToFirst(out TData value)
    {
        if (!_dataItems.Any())
        {
            value = default;
            return false;
        }

        var first = _dataItems.First();
        Remove(first);
        value = first;
        return true;
    }

    public virtual bool Remove(TData data)
    {
        if (!_viewMap.TryGetValue(data, out var view)) return false;

        if (view != null && view.gameObject) Destroy(view.gameObject);

        _viewMap.Remove(data);
        _dataItems.Remove(data);
        OnChanged?.Invoke();
        OnChangedInternal();
        return true;
    }

    public IEnumerable<TView> GetViews() => _dataItems.Select(d => _viewMap[d]);
}
