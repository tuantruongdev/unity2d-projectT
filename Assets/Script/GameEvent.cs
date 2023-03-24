using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents {
    List<Event> events = new List<Event>();
   public GameEvents()
    {
        events = new List<Event>();
    }
    public void addEvent(Event newEvent)
    {
        events.Add(newEvent);
    }

    public Event GetEvent(int pos)
    {
        return events.ElementAt(pos);
    }

    public void removeMessage(string key) {
    //todo
   
    }
    
    public List<Event> getEvents() { 
        return events;
    }
 
}
