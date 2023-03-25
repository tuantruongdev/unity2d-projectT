using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event
{
    public int eventId {  get;private set; }
    public Event(int eventId)
    {
        this.eventId = eventId;
    }
}
public class EventHit: Event { 

    public int weaponId { get; private set; }
    public int damage { get; private set; }
    public float freezeTime { get; private set; }

   public EventHit(int eventId,int weaponId, int damage,float freezeTime) : base(eventId)
    {
       this.weaponId = weaponId;
       this.damage = damage;   
       this.freezeTime = freezeTime;
    }

}

