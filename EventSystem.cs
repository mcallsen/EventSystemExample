using System;
using System.Collections.Generic;

public class EventSystem {
    List<Action<EventData>> events;
    public EventSystem()
    {
        events = new List<Action<EventData>>();
    }

    public void Subscribe<T>(Action<T> listener) where T: EventData {       
        events[eventType].Add(listener);
    }

    public void Unsubscribe<T>(Action<T> listener) where T: EventData {
        events[eventType].Remove(listener);
    }

    public void TriggerEvent<T>(T eventData) where T: EventData {
        foreach (Action<T> listener in events) {
            listener(eventData);
        }
    }
}

void EventHandler(DerivedData data) {}

public class EventData {}
public class DerivedData: EventData {}

