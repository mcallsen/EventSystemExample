using System;
using System.Collections.Generic;

public class EventSystem {
    Dictionary<EventType, List<Action<EventData>>> events;
    public EventSystem()
    {
        events = new Dictionary<EventType, List<Action<EventData>>>();
    }

    public void Subscribe<T>(EventType eventType, Action<T> listener) where T: EventData {
        if (events.ContainsKey(eventType) == false) {
            events[eventType] = new List<Action<EventData>>();
        }
        
        events[eventType].Add(listener);
    }

    public void Unsubscribe<T>(EventType eventType, Action<T> listener) where T: EventData {
        if (events.ContainsKey(eventType) == true) {
            events[eventType].Remove(listener);
        }
    }

    public void TriggerEvent<T>(EventType eventType, T eventData) where T: EventData {
        if (events.ContainsKey(eventType)) {

            foreach (Action<T> listener in events[eventType]) {
                listener(eventData);
            }
        }
    }
}

void EventHandler(DerivedData data) {}

public class EventData {}
public class DerivedData: EventData {}

public enum EventType {
    TEST_EVENT = 0
}
