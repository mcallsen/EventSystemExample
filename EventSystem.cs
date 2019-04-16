using System;
using System.Collections.Generic;

public static class EventSystem {
    static List<Action<EventData>> events = new List<Action<EventData>>();

    public static void Subscribe<T>(Action<T> listener) where T: EventData {       
        events.Add(listener);
    }

    public static void Unsubscribe<T>(Action<T> listener) where T: EventData {
        events.Remove(listener);
    }

    public static void TriggerEvent<T>(T eventData) where T: EventData {
        foreach (Action<T> listener in events) {
            listener(eventData);
        }
    }
}

public abstract class EventData {}
public class DerivedData: EventData {}


public static class ActionTools {
    public static Action<TDerived> DownCast<TBase, TDerived>( Action<TBase> myAction ) where TDerived: TBase, new()  {
        if (myAction == null) {
            return null;
        }
        return new Action<TDerived>( o => myAction(o));
    }

    public static Action<TBase> UpCast<TDerived, TBase>( Action<TDerived> myAction ) where TDerived: TBase, new()  {
        if (myAction == null) {
            return null;
        }
        return new Action<TBase>( o => myAction((TDerived)o));
    }
}


public static class EventSystem2 {
    static List<Action<EventData>> events = new List<Action<EventData>>();

    public static void Subscribe<T>(Action<T> listener) where T: EventData, new() {       
        events.Add(ActionTools.UpCast<T, EventData>(listener));
    }

    public static void Unsubscribe<T>(Action<T> listener) where T: EventData, new() {
        // This probably won't even work, because I am not sure whether the upcasted 
        // version of the listener can be found in the list.
        events.Remove(ActionTools.UpCast<T, EventData>(listener));
    }

    public static void TriggerEvent<T>(T eventData) where T: EventData {
        foreach (Action<T> listener in events) {
            listener(eventData);
        }
    }
}

public static class EventSystem3 {
    
    public static void TriggerEvent<T>(EventType et, T eventData ) {
        GenericEventSystem<T>.TriggerEvent(et, eventData);
    }
}

public static class GenericEventSystem<T> {
    static Dictionary<EventType, List<Action<T>>> events = new Dictionary<EventType, List<Action<T>>>();

    public static void Subscribe(EventType et, Action<T> listener) {       
        if (events.ContainsKey(et) == false) {
            events[et] = new List<Action<T>>();
        }
        events[et].Add(listener);
    }

    public static void Unsubscribe(EventType et, Action<T> listener) {
        if (events.ContainsKey(et) == true) {
            events[et].Remove(listener);
        }
    }

    public static void TriggerEvent(EventType et, T eventData) {
        if (events.ContainsKey(et) == true) {
            foreach (Action<T> listener in events[et]) {
                listener(eventData);
            }
        }
    }
}

public static class EventSystem4 {
    
    public static void TriggerEvent<T>( object sender, EventType et, T eventData ) {
        GenericEventArgs<T> e = EventArgsFactory.New(eventData);
        GenericEventSystem2<T>.TriggerEvent(sender, et, e);
    }
}

public static class GenericEventSystem2<T> {
    static Dictionary<EventType, EventHandler<GenericEventArgs<T>>> events = new Dictionary<EventType, EventHandler<GenericEventArgs<T>>>();

    public static void Subscribe(EventType et, EventHandler<GenericEventArgs<T>> listener) {       
        if (events.ContainsKey(et) == false) {
            events[et] = null;
        }
        events[et] += listener;
    }

    public static void Unsubscribe(EventType et, EventHandler<GenericEventArgs<T>> listener) {
        if (events.ContainsKey(et) == true) {
            events[et] -= listener;
        }
    }

    public static void TriggerEvent(object sender, EventType et, GenericEventArgs<T> e) {
        if (events.ContainsKey(et) == true) {
            events[et]?.Invoke(sender, e);
        }
    }
}

public static class EventArgsFactory {
    public static GenericEventArgs<T> New<T> (T arg) {
        GenericEventArgs<T> newEventArgs = new GenericEventArgs<T>(arg);
        return newEventArgs;
    }
}

public class GenericEventArgs<T>: EventArgs {

    public GenericEventArgs(T arg) {
        eventArgs = arg;
    }
    public T eventArgs {get; set;}
}

public enum EventType {
    TEST_EVENT = 0
}
