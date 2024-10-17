using System.Collections.Generic;
using System;
using System.Linq;
using BookHub.Models.Dispatching.Events;

namespace BookHub.Models.Dispatching;

public sealed class EventDispatcher : IEventDispatcher
{
    /// <inheritdoc/>
    public void Publish<T>(T publishingEvent) where T : IDomainEvent
    {
        ArgumentNullException.ThrowIfNull(publishingEvent);

        foreach (var handler in
            _eventHandlers.Where(kv => kv.Key.IsAssignableFrom(typeof(T)))
                .SelectMany(kv => kv.Value).Cast<Action<T>>())
        {
            handler(publishingEvent);
        }
    }

    /// <inheritdoc/>
    public void Register<T>(Action<T> eventHandler) where T : IDomainEvent
    {
        ArgumentNullException.ThrowIfNull(eventHandler);

        var eventType = typeof(T);

        if (_eventHandlers.TryGetValue(eventType, out var handlers))
        {
            handlers.Add(eventHandler);
        }
        else
        {
            _eventHandlers[eventType] = [eventHandler];
        }
    }

    private readonly Dictionary<Type, List<Delegate>> _eventHandlers = [];
}